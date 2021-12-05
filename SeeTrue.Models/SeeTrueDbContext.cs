using System;
using Microsoft.EntityFrameworkCore;

namespace SeeTrue.Models
{
    public interface ISeeTrueDbContext { }

    public abstract class SeeTrueDbContext : DbContext, ISeeTrueDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLogEntry> AuditLogEntries { get; set; }

        public SeeTrueDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
