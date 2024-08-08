namespace Users.Api.Service.Settings;

public class MongoDbSettings
{
    /// <summary>
    /// Ubicación de la instancia de MongoDB
    /// </summary>
    public string? Host { get; init; }

    /// <summary>
    /// Puerto de la instancia de MongoDB
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Usuario de conexión a la instancia de MongoDB
    /// </summary>
    public string? UserName { get; init; }

    /// <summary>
    /// Clave de acceso del usuario de conexión a la instancia de MongoDB
    /// </summary>
    public string? UserPass { get; init; }

    /// <summary>
    /// Cadena de configuración para la conxión a la instancia de MongoDB
    /// </summary>
    public string ConnectionString => $"mongodb://{UserName}:{UserPass}@{Host}:{Port}";
}