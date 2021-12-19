using System;
using Microsoft.Extensions.DependencyInjection;
using SeeTrue.Infrastructure.Services;

namespace SeeTrue.Infrastructure
{
    public static class ServiceExtension
    {
        public static void AddSeeTrue(this IServiceCollection services)
        {
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<ICommandService, CommandService>();
        }
    }
}
