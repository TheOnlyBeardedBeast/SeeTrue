using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Models;
using SeeTrue.Utils.Services;

namespace SeeTrue.Tests
{
    public class DecodedJWT
    {
        public Guid sub { get; set; }
        public string aud { get; set; }
        public string iss { get; set; }
        public int exp { get; set; }
        public Guid lid { get; set; }
        public string role { get; set; }
        public Guid gid { get; set; }
    }

public class Mailer : IMailService
    {
        public (string, string) mail;

        public Task NotifyEmailChange(User user, string email)
        {
            mail = (user.Email, email);

            return Task.CompletedTask;
        }

        public Task NotifyMagicLink(User user, string token)
        {
            mail = (user.Email, token);

            return Task.CompletedTask;
        }

        public Task NotifyRecovery(User user)
        {
            mail = (user.Email, user.RecoveryToken);

            return Task.CompletedTask;
        }

        public Task Send(string address, string code)
        {
            mail = (address, code);

            return Task.CompletedTask;
        }
    }

    public class DB : SeeTrueDbContext
    {
        public DB(DbContextOptions options) : base(options)
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

    public class SeeTrueFixture
    {
        public readonly IQueryService queries;
        public readonly ICommandService commands;
        public readonly DB db;
        public readonly Mailer mailer;
        public readonly IMemoryCache cache;
        public IMemoryCache testCache = new MemoryCache(new MemoryCacheOptions());

        public SeeTrueFixture()
        {
            var type = typeof(Env);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            var builder = new DbContextOptionsBuilder<DB>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new DB(builder.Options);
            this.cache = new MemoryCache(new MemoryCacheOptions());
            this.mailer = new Mailer();

            this.queries = new QueryService(db, this.cache);
            this.commands = new CommandService(db, this.mailer, this.cache);
        }
    }
}
