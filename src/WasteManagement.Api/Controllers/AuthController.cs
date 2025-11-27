using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WasteManagement.Api.Contracts;
using WasteManagement.Application.Abstractions;
using WasteManagement.Application.Security;

namespace WasteManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly AuthSettings _settings;

    public AuthController(ITokenService tokenService, IOptions<AuthSettings> options)
    {
        _tokenService = tokenService;
        _settings = options.Value;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GenerateToken([FromBody] LoginRequest request)
    {
        var user = _settings.Users.FirstOrDefault(u =>
            u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == request.Password);

        if (user is null)
        {
            return Unauthorized(new { message = "Credenciais inválidas" });
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new
        {
            access_token = token,
            role = user.Role,
            expires_in = 6 * 60 * 60
        });
    }
}
