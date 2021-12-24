﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public record AdminUpdateUserRequest
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("confirm")]
        public bool? Confirm { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("appMetaData")]
        public Dictionary<string, object> AppMetaData { get; set; }
        [JsonPropertyName("userMetaData")]
        public Dictionary<string, object> UserMetaData { get; set; }
    }
}
