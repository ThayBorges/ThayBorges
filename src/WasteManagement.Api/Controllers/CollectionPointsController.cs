using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WasteManagement.Application.Contracts.CollectionPoints;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionPointsController : ControllerBase
{
    private readonly ICollectionPointService _collectionPointService;

    public CollectionPointsController(ICollectionPointService collectionPointService)
    {
        _collectionPointService = collectionPointService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<CollectionPointListItemViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] CollectionPointFilter filter, [FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var response = await _collectionPointService.GetAsync(filter, pagination, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _collectionPointService.GetByIdAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCollectionPointRequest request, CancellationToken cancellationToken)
    {
        var id = await _collectionPointService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCollectionPointRequest request, CancellationToken cancellationToken)
    {
        await _collectionPointService.UpdateAsync(id, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        await _collectionPointService.DeactivateAsync(id, cancellationToken);
        return NoContent();
    }
}
