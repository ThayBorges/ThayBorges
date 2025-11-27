namespace WasteManagement.Application.Exceptions;

public sealed class NotFoundException(string resource, string key)
    : AppException($"{resource} com identificador {key} não foi encontrado.", "not_found")
{
}
