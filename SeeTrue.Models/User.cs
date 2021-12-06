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
        public Guid? InstanceID { get; set; }
        [Key]
        public Guid Id { get; set; }
        public string Aud { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string EncryptedPassword { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? InvitedAt { get; set; }
        public string ConfirmationToken { get; set; }
        public DateTime? ConfirmationSentAt { get; set; }
        public string RecoveryToken { get; set; }
        public DateTime? RecoverySentAt { get; set; }
        public string EmailChangeToken { get; set; }
        public string EmailChange { get; set; }
        public DateTime? EmailChangeSentAt { get; set; }
        public DateTime? LastSignInAt { get; set; }
        [JsonIgnore]
        [Column(TypeName = "jsonb")]
        public string AppMetaDataJson
        {
            get => JsonSerializer.Serialize(this.AppMetaData);
            init {
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
	}
}
