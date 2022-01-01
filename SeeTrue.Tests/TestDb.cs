using System;
using Microsoft.EntityFrameworkCore;
using SeeTrue.Models;

namespace SeeTrue.Tests
{
    public class TestDb : SeeTrueDbContext
    {
        public TestDb(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(x => x.Ignore(x => x.AppMetaData));
            modelBuilder.Entity<User>(x => x.Ignore(x => x.UserMetaData));
            modelBuilder.Entity<AuditLogEntry>(x => x.Ignore(x => x.Payload));
            base.OnModelCreating(modelBuilder);
        }
    }
}
