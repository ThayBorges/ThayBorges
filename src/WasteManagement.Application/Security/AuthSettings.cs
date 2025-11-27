namespace WasteManagement.Application.Security;

public class AuthSettings
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public IList<ApiUserCredential> Users { get; set; } = new List<ApiUserCredential>();
}

public class ApiUserCredential
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
