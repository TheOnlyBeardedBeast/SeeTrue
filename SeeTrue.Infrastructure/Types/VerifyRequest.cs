using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record VerifyRequest
    {
        [JsonPropertyName("type")]
        public string Type { get; init; }
        [JsonPropertyName("token")]
        public string Token { get; init; }
        [JsonPropertyName("password")]
        public string Password { get; init; }
        [JsonPropertyName("name")]
        public string Name { get; init; }
    }
}