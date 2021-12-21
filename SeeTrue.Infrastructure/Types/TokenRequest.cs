using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record TokenRequest
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
