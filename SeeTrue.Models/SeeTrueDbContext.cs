using System;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SeeTrue.Models
{
    public interface ISeeTrueDbContext { }

    public abstract class SeeTrueDbContext : DbContext, ISeeTrueDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLogEntry> AuditLogEntries { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Mail> Mails { get; set; }

        public SeeTrueDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(e => new { e.Email, e.InstanceID, e.Aud }).IsUnique();
            modelBuilder.Entity<User>().Property(e => e.AppMetaData).HasConversion(
            v => JsonSerializer.Serialize<Dictionary<string, string>>(v, new JsonSerializerOptions()),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v,new JsonSerializerOptions()));
            modelBuilder.Entity<User>().Property(e => e.UserMetaData).HasConversion(
            v => JsonSerializer.Serialize<Dictionary<string, string>>(v, new JsonSerializerOptions()),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v,new JsonSerializerOptions()));
            modelBuilder.Entity<AuditLogEntry>().Property(e => e.Payload).HasConversion(
            v => JsonSerializer.Serialize<Dictionary<string, string>>(v, new JsonSerializerOptions()),
            v => JsonSerializer.Deserialize<Dictionary<string, string>>(v,new JsonSerializerOptions()));
           
            modelBuilder.Entity<Mail>().HasIndex(e => new { e.InstanceId, e.Language, e.Type, e.Audience }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
