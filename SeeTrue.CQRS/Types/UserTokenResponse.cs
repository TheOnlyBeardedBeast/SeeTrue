using System;
using System.Text.Json.Serialization;
using SeeTrue.Models;

namespace SeeTrue.CQRS.Types
{
    public record UserTokenResponse : TokenResponse
    {
        [JsonPropertyName("user")]
        public User User { get; init; }
    }
}
