using System;
using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; init; } = "Bearer";

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; } = 3600;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; init; }
    }
}
