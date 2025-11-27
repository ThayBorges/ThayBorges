using WasteManagement.Application.Contracts.Alerts;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Application.Interfaces;

public interface IAlertService
{
    Task<PaginatedResponse<WasteAlertViewModel>> GetAsync(Guid? collectionPointId, PaginationQuery pagination, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(CreateAlertRequest request, CancellationToken cancellationToken);
    Task ResolveAsync(Guid alertId, ResolveAlertRequest request, CancellationToken cancellationToken);
}
