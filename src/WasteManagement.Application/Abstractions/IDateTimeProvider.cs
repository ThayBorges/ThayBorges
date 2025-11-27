namespace WasteManagement.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
