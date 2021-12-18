using System;
using System.Text.Json.Serialization;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Types
{
    public record UserTokenResponse : TokenResponse
    {
        [JsonPropertyName("user")]
        public User User { get; init; }
    }
}
