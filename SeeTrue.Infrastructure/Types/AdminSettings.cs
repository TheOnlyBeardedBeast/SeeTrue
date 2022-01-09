using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;

namespace SeeTrue.Infrastructure.Types
{
    public record AdminSettings
    {
        [JsonPropertyName("languages")]
        public IEnumerable<string> Languages { get; init; } = Env.Languages;

        [JsonPropertyName("audiences")]
        public IEnumerable<string> Audiences { get; init; } = Env.Audiences;

        [JsonPropertyName("emailTypes")]
        public IDictionary<string, int> EmailTypes { get; init; } = Helpers.ConvertToDictionary<NotificationType>();
    }
}
