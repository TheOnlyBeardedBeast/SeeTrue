using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace SeeTrue.Models
{
    public class User
    {
        public Guid InstanceID { get; set; }
        [Key]
        public Guid Id { get; set; }
        public string Aud { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        [JsonIgnore]
        public string EncryptedPassword { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? InvitedAt { get; set; }
        [JsonIgnore]
        public string ConfirmationToken { get; set; }
        [JsonIgnore]
        public DateTime? ConfirmationSentAt { get; set; }
        [JsonIgnore]
        public string RecoveryToken { get; set; }
        public DateTime? RecoverySentAt { get; set; }
        [JsonIgnore]
        public string EmailChangeToken { get; set; }
        public string EmailChange { get; set; }
        public DateTime? EmailChangeSentAt { get; set; }
        public DateTime? LastSignInAt { get; set; }
        [JsonIgnore]
        [Column(TypeName = "jsonb")]
        public string AppMetaDataJson
        {
            get => JsonSerializer.Serialize(this.AppMetaData);
            init
            {
                this.AppMetaData = JsonSerializer.Deserialize<Dictionary<string, object>>(value);
            }
        }
        [NotMapped]
        public Dictionary<string, object> AppMetaData { get; set; }
        [JsonIgnore]
        [Column(TypeName = "jsonb")]
        public string UserMetaDataJson
        {
            get => JsonSerializer.Serialize(this.UserMetaData);
            init
            {
                this.UserMetaData = JsonSerializer.Deserialize<Dictionary<string, object>>(value);
            }
        }
        [NotMapped]
        public Dictionary<string, object> UserMetaData { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
