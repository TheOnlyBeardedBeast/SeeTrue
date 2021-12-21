namespace SeeTrue.Infrastructure.Types
{
    public record HealthCheckResponse(string Name, float version, string Description);
}