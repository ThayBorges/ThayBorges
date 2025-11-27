using WasteManagement.Application.Security;

namespace WasteManagement.Application.Abstractions;

public interface ITokenService
{
    string GenerateToken(ApiUserCredential user);
}
