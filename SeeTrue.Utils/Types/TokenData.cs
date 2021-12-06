using System;
using System.Text.Json.Serialization;

namespace SeeTrue.Utils.Types
{
    public record TokenData
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
