using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record SignUpRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("userMetaData")]
        public Dictionary<string, object> UserMetaData { get; set; }
    }
}
