using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WasteManagement.Application.Contracts.Alerts;
using WasteManagement.Application.Contracts.Common;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertsController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<WasteAlertViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAsync([FromQuery] Guid? collectionPointId, [FromQuery] PaginationQuery pagination, CancellationToken cancellationToken)
    {
        var response = await _alertService.GetAsync(collectionPointId, pagination, cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAlertRequest request, CancellationToken cancellationToken)
    {
        var id = await _alertService.CreateAsync(request, cancellationToken);
        return Created($"/api/alerts/{id}", new { id });
    }

    [HttpPatch("{id:guid}/resolve")]
    [Authorize(Policy = "PlannerOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResolveAsync(Guid id, [FromBody] ResolveAlertRequest request, CancellationToken cancellationToken)
    {
        await _alertService.ResolveAsync(id, request, cancellationToken);
        return NoContent();
    }
}
