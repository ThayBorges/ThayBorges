using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WasteManagement.Application.Contracts.CollectionRequests;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Api.Controllers;

[ApiController]
[Route("api/collection-requests")]
public class CollectionRequestsController : ControllerBase
{
    private readonly ICollectionRequestService _collectionRequestService;

    public CollectionRequestsController(ICollectionRequestService collectionRequestService)
    {
        _collectionRequestService = collectionRequestService;
    }

    [HttpGet("collection-point/{collectionPointId:guid}")]
    [ProducesResponseType(typeof(PaginatedResponse<CollectionRequestViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForPointAsync(Guid collectionPointId, [FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var response = await _collectionRequestService.GetForPointAsync(collectionPointId, pagination, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCollectionRequest request, CancellationToken cancellationToken)
    {
        var id = await _collectionRequestService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetForPointAsync), new { collectionPointId = request.CollectionPointId }, new { id });
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStatusAsync(Guid id, [FromBody] UpdateRequestStatusRequest request, CancellationToken cancellationToken)
    {
        await _collectionRequestService.UpdateStatusAsync(id, request, cancellationToken);
        return NoContent();
    }
}
