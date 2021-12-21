using System;
using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record TokenRequest
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
