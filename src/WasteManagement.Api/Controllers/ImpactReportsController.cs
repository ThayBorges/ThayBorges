using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WasteManagement.Application.Contracts.Impact;
using WasteManagement.Application.Interfaces;
using WasteManagement.Application.ViewModels;

namespace WasteManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImpactReportsController : ControllerBase
{
    private readonly IImpactReportService _impactReportService;

    public ImpactReportsController(IImpactReportService impactReportService)
    {
        _impactReportService = impactReportService;
    }

    [HttpGet("snapshots")]
    [Authorize(Policy = "AuditorOnly")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ImpactSnapshotViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSnapshotsAsync([FromQuery] ImpactReportQuery query, CancellationToken cancellationToken)
    {
        var response = await _impactReportService.GetSnapshotsAsync(query, cancellationToken);
        return Ok(response);
    }
}
