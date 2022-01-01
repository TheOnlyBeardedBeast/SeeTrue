using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.Tests
{
    public class SeeTrueFixture
    {
        public readonly IQueryService queries;
        public readonly ICommandService commands;
        public readonly TestDb db;
        public readonly FakeMailer mailer;
        public readonly IMemoryCache cache;
        public IMemoryCache testCache = new MemoryCache(new MemoryCacheOptions());

        public SeeTrueFixture()
        {
            var type = typeof(Env);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            var builder = new DbContextOptionsBuilder<TestDb>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            this.db = new TestDb(builder.Options);
            this.cache = new MemoryCache(new MemoryCacheOptions());
            this.mailer = new FakeMailer();

            this.queries = new QueryService(db, this.cache);
            this.commands = new CommandService(db, this.mailer, this.cache);
        }
    }
}
