using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record HealthCheckResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }
        [JsonPropertyName("version")]
        public float Version { get; init; }
        [JsonPropertyName("description")]
        public string Description { get; init; }
    };
}