using Microsoft.EntityFrameworkCore;
using WasteManagement.Application.Abstractions;
using WasteManagement.Application.Contracts.CollectionRequests;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.Exceptions;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.Mapping;
using WasteManagement.Application.ViewModels;
using WasteManagement.Domain.Entities;
using WasteManagement.Infrastructure.Persistence;

namespace WasteManagement.Infrastructure.Services;

public class CollectionRequestService : ICollectionRequestService
{
    private readonly WasteManagementDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CollectionRequestService(WasteManagementDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaginatedResponse<CollectionRequestViewModel>> GetForPointAsync(Guid collectionPointId, PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = _dbContext.CollectionRequests
            .AsNoTracking()
            .Where(x => x.CollectionPointId == collectionPointId);

        var total = await query.LongCountAsync(cancellationToken);
        var skip = (pagination.Page - 1) * pagination.PageSize;

        var items = await query
            .OrderByDescending(x => x.RequestedAtUtc)
            .Skip(skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        var viewModels = items.Select(x => x.ToViewModel()).ToList();
        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PaginatedResponse<CollectionRequestViewModel>(
            viewModels,
            new PaginationMeta
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalItems = total,
                TotalPages = totalPages
            });
    }

    public async Task<Guid> CreateAsync(CreateCollectionRequest request, CancellationToken cancellationToken)
    {
        var pointExists = await _dbContext.CollectionPoints.AnyAsync(x => x.Id == request.CollectionPointId, cancellationToken);
        if (!pointExists)
        {
            throw new NotFoundException("CollectionPoint", request.CollectionPointId.ToString());
        }

        var entity = new CollectionRequest
        {
            CollectionPointId = request.CollectionPointId,
            Priority = request.Priority,
            EstimatedVolumeKg = request.EstimatedVolumeKg,
            ScheduledForUtc = request.ScheduledForUtc,
            RequestedBy = request.RequestedBy,
            Notes = request.Notes,
            RequestedAtUtc = _dateTimeProvider.UtcNow
        };

        await _dbContext.CollectionRequests.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    public async Task UpdateStatusAsync(Guid requestId, UpdateRequestStatusRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.CollectionRequests.FirstOrDefaultAsync(x => x.Id == requestId, cancellationToken);
        if (entity is null)
        {
            throw new NotFoundException("CollectionRequest", requestId.ToString());
        }

        entity.Status = request.Status;
        entity.CompletedAtUtc = request.CompletedAtUtc;
        entity.Notes = string.IsNullOrWhiteSpace(request.Notes) ? entity.Notes : request.Notes;
        entity.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
