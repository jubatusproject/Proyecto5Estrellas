namespace Users.Api.Gateway;

public static class ApiEndPoints
{
    public const string HealthCheckGatewayLive = "api/gateway/health/live";
}

public static class ApiMessages
{
    public const string SecurityDefinitionName = "Bearer";
    public const string RateLimiterPolicyName = "fixed";
}