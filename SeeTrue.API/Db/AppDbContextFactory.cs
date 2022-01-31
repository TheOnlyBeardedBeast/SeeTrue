using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SeeTrue.API.DB
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        // Run export env variable before running the migration
        // export SEETRUE_DB_CONNECTION="Host=localhost;Database=seetrue;Username=postgres;Password=postgres"
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SEETRUE_DB_CONNECTION"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
