using System.Text.Json.Serialization;
using UserMetaData = System.Collections.Generic.Dictionary<string, object>;

namespace SeeTrue.Infrastructure.Types
{
    public record UserUpdateRequest
    {
        [JsonPropertyName("password")]
        public string Password { get; init; }
        [JsonPropertyName("email")]
        public string Email { get; init; }
        [JsonPropertyName("userMetaData")]
        public UserMetaData UserMetaData { get; init; }
    }
}