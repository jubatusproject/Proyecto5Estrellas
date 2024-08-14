namespace Users.Api.Gateway.Settings;

public class JwtSettings
{
    /// <summary>
    /// 
    /// </summary>
    public string? JwtKey { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? JwtIssuer { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? JwtAudience { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool ValidateIssuer { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool ValidateAudience { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool ValidateLifetime { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public bool ValidateIssuerSigningKey { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? AuthUser { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? AuthPass { get; init; }
}