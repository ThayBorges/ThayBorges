using WasteManagement.Application.Contracts.Impact;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Application.Interfaces;

public interface IImpactReportService
{
    Task<IReadOnlyCollection<ImpactSnapshotViewModel>> GetSnapshotsAsync(ImpactReportQuery query, CancellationToken cancellationToken);
}
