using Microsoft.EntityFrameworkCore;
using WasteManagement.Application.Abstractions;
using WasteManagement.Application.Contracts.Alerts;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.Exceptions;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.Mapping;
using WasteManagement.Application.ViewModels;
using WasteManagement.Domain.Entities;
using WasteManagement.Infrastructure.Persistence;

namespace WasteManagement.Infrastructure.Services;

public class AlertService : IAlertService
{
    private readonly WasteManagementDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AlertService(WasteManagementDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaginatedResponse<WasteAlertViewModel>> GetAsync(Guid? collectionPointId, PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var query = _dbContext.WasteAlerts
            .AsNoTracking()
            .AsQueryable();

        if (collectionPointId.HasValue)
        {
            query = query.Where(x => x.CollectionPointId == collectionPointId.Value);
        }

        var total = await query.LongCountAsync(cancellationToken);
        var skip = (pagination.Page - 1) * pagination.PageSize;

        var items = await query
            .OrderByDescending(x => x.TriggeredAtUtc)
            .Skip(skip)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        var viewModels = items.Select(x => x.ToViewModel()).ToList();
        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PaginatedResponse<WasteAlertViewModel>(
            viewModels,
            new PaginationMeta
            {
                Page = pagination.Page,
                PageSize = pagination.PageSize,
                TotalItems = total,
                TotalPages = totalPages
            });
    }

    public async Task<Guid> CreateAsync(CreateAlertRequest request, CancellationToken cancellationToken)
    {
        var pointExists = await _dbContext.CollectionPoints.AnyAsync(x => x.Id == request.CollectionPointId, cancellationToken);
        if (!pointExists)
        {
            throw new NotFoundException("CollectionPoint", request.CollectionPointId.ToString());
        }

        var alert = new WasteAlert
        {
            CollectionPointId = request.CollectionPointId,
            Severity = request.Severity,
            Message = request.Message,
            TriggeredAtUtc = _dateTimeProvider.UtcNow
        };

        await _dbContext.WasteAlerts.AddAsync(alert, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return alert.Id;
    }

    public async Task ResolveAsync(Guid alertId, ResolveAlertRequest request, CancellationToken cancellationToken)
    {
        var alert = await _dbContext.WasteAlerts.FirstOrDefaultAsync(x => x.Id == alertId, cancellationToken);
        if (alert is null)
        {
            throw new NotFoundException("WasteAlert", alertId.ToString());
        }

        alert.Resolved = request.Resolved;
        alert.ResolvedAtUtc = request.Resolved ? _dateTimeProvider.UtcNow : null;
        alert.UpdatedAtUtc = _dateTimeProvider.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
