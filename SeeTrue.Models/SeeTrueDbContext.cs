using System;
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
            modelBuilder.Entity<User>().Property<string>("UserMetaDataJson");
            modelBuilder.Entity<User>().Property<string>("AppMetaDataJson");

            base.OnModelCreating(modelBuilder);
        }
    }
}
