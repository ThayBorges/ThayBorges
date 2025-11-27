using WasteManagement.Application.Contracts.CollectionPoints;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Application.Interfaces;

public interface ICollectionPointService
{
    Task<PaginatedResponse<CollectionPointListItemViewModel>> GetAsync(CollectionPointFilter filter, PaginationQuery pagination, CancellationToken cancellationToken);
    Task<CollectionPointViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(CreateCollectionPointRequest request, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, UpdateCollectionPointRequest request, CancellationToken cancellationToken);
    Task DeactivateAsync(Guid id, CancellationToken cancellationToken);
}
