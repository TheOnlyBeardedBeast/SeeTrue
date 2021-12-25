using System;
using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public class InviteRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
