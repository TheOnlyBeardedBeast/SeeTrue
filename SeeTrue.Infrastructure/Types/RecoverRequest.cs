using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record RecoverRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}