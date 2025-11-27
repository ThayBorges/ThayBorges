using WasteManagement.Application.Contracts.CollectionRequests;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Application.Interfaces;

public interface ICollectionRequestService
{
    Task<PaginatedResponse<CollectionRequestViewModel>> GetForPointAsync(Guid collectionPointId, PaginationQuery pagination, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(CreateCollectionRequest request, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid requestId, UpdateRequestStatusRequest request, CancellationToken cancellationToken);
}
