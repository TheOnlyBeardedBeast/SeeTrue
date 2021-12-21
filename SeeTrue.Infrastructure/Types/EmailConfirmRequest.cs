using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record EmailConfirmRequest
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}