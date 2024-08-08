namespace Users.Api.Service.Settings;

public class JwtSettings
{
    public string JwtKey { get; set; } = "4a10573510aa4ba09271b55d7044fa51";
    public string JwtIssuer { get; set; } = "https://www.uuidgenerator.net";
    public string JwtAudience { get; set; } = "uuidgenerator";
}