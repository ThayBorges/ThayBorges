using WasteManagement.Domain.Enums;

namespace WasteManagement.Domain.Entities;

public class CollectionPoint : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public MaterialCategory MaterialCategory { get; set; }
    public double CapacityKg { get; set; }
    public double CurrentLoadKg { get; set; }
    public bool SupportsIoTSensors { get; set; }
    public bool IsActive { get; set; } = true;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime? LastPickupUtc { get; set; }
    public DateTime? NextInspectionUtc { get; set; }

    public ICollection<CollectionRequest> Requests { get; set; } = new List<CollectionRequest>();
    public ICollection<WasteAlert> Alerts { get; set; } = new List<WasteAlert>();
}
