using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record MagicLinkRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    };
}