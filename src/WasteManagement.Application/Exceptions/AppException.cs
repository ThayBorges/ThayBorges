namespace WasteManagement.Application.Exceptions;

public abstract class AppException(string message, string? code = null) : Exception(message)
{
    public string? Code { get; } = code;
}
