namespace WasteManagement.Application.Contracts.Alerts;

public record ResolveAlertRequest
{
    public required bool Resolved { get; init; }
    public string Notes { get; init; } = string.Empty;
}
