using Microsoft.EntityFrameworkCore;
using WasteManagement.Application.Abstractions;
using WasteManagement.Application.Contracts.CollectionPoints;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.Exceptions;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.Mapping;
using WasteManagement.Application.ViewModels;
using WasteManagement.Domain.Entities;
using WasteManagement.Infrastructure.Persistence;

namespace WasteManagement.Infrastructure.Services;

public class CollectionPointService : ICollectionPointService
{
    private readonly WasteManagementDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CollectionPointService(WasteManagementDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaginatedResponse<CollectionPointListItemViewModel>> GetAsync(CollectionPointFilter filter, PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = _dbContext.CollectionPoints
            .Include(x => x.Alerts)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Neighborhood))
        {
            query = query.Where(x => x.Neighborhood.ToLower().Contains(filter.Neighborhood.ToLower()));
        }

        if (filter.Material.HasValue)
        {
            query = query.Where(x => x.MaterialCategory == filter.Material);
        }

        if (filter.OnlyActive == true)
        {
            query = query.Where(x => x.IsActive);
        }

        if (filter.MinFillPercentage.HasValue)
        {
            var min = filter.MinFillPercentage.Value;
            query = query.Where(x => x.CapacityKg > 0 && (x.CurrentLoadKg / x.CapacityKg) * 100 >= min);
        }

        if (filter.OnlyCriticalLoad == true)
        {
            query = query.Where(x => x.CapacityKg > 0 && (x.CurrentLoadKg / x.CapacityKg) >= 0.85);
        }

        var total = await query.LongCountAsync(cancellationToken);

        var skip = (pagination.Page - 1) * pagination.PageSize;

        var items = await query
            .OrderByDescending(x => x.CapacityKg == 0 ? 0 : x.CurrentLoadKg / x.CapacityKg)
            .ThenBy(x => x.Name)
            .Skip(skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        var viewModels = items
            .Select(x => x.ToListItemViewModel())
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PaginatedResponse<CollectionPointListItemViewModel>(
            viewModels,
            new PaginationMeta
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalItems = total,
                TotalPages = totalPages
            });
    }

    public async Task<CollectionPointViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CollectionPoints
            .Include(x => x.Alerts)
            .Include(x => x.Requests)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity?.ToViewModel();
    }

    public async Task<Guid> CreateAsync(CreateCollectionPointRequest request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.CollectionPoints.AnyAsync(x => x.Code == request.Code, cancellationToken);
        if (exists)
        {
            throw new ConflictException($"Já existe um ponto com o código {request.Code}.");
        }

        var entity = new CollectionPoint
        {
            Code = request.Code,
            Name = request.Name,
            Neighborhood = request.Neighborhood,
            MaterialCategory = request.MaterialCategory,
            CapacityKg = request.CapacityKg,
            CurrentLoadKg = request.CurrentLoadKg,
            SupportsIoTSensors = request.SupportsIoTSensors,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            NextInspectionUtc = request.NextInspectionUtc
        };

        await _dbContext.CollectionPoints.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateCollectionPointRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CollectionPoints.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException("CollectionPoint", id.ToString());
        }

        entity.Name = request.Name;
        entity.Neighborhood = request.Neighborhood;
        entity.MaterialCategory = request.MaterialCategory;
        entity.CapacityKg = request.CapacityKg;
        entity.CurrentLoadKg = request.CurrentLoadKg;
        entity.SupportsIoTSensors = request.SupportsIoTSensors;
        entity.IsActive = request.IsActive;
        entity.Latitude = request.Latitude;
        entity.Longitude = request.Longitude;
        entity.NextInspectionUtc = request.NextInspectionUtc;
        entity.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CollectionPoints.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException("CollectionPoint", id.ToString());
        }

        entity.IsActive = false;
        entity.UpdatedAtUtc = _dateTimeProvider.UtcNow;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
