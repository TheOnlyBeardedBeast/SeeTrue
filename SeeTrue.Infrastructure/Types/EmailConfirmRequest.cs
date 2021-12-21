using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record EmailConfirmRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}