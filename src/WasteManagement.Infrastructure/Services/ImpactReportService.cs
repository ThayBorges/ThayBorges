using Microsoft.EntityFrameworkCore;
using WasteManagement.Application.Contracts.Impact;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.Mapping;
using WasteManagement.Application.ViewModels;
using WasteManagement.Infrastructure.Persistence;

namespace WasteManagement.Infrastructure.Services;

public class ImpactReportService : IImpactReportService
{
    private readonly WasteManagementDbContext _dbContext;

    public ImpactReportService(WasteManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ImpactSnapshotViewModel>> GetSnapshotsAsync(ImpactReportQuery query, CancellationToken cancellationToken)
    {
        var snapshots = _dbContext.ImpactSnapshots
            .AsNoTracking()
            .AsQueryable();

        if (query.Start.HasValue)
        {
            snapshots = snapshots.Where(x => x.SnapshotDate >= query.Start.Value);
        }

        if (query.End.HasValue)
        {
            snapshots = snapshots.Where(x => x.SnapshotDate <= query.End.Value);
        }

        var data = await snapshots
            .OrderByDescending(x => x.SnapshotDate)
            .Take(90)
            .ToListAsync(cancellationToken);

        return data.Select(x => x.ToViewModel()).ToList();
    }
}
