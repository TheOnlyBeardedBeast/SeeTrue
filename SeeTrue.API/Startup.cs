using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SeeTrue.API.Db;
using SeeTrue.API.Filters;
using SeeTrue.API.Services;
using SeeTrue.Infrastructure;
using SeeTrue.Models;
using SeeTrue.Utils.Services;

namespace SeeTrue.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDbContext<ISeeTrueDbContext, AppDbContext>(options =>
                 options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers(
                opt =>
                {
                    opt.Filters.Add<TransactionFilter>();
                    opt.Filters.Add<SeeTrueExceptionFilter>();
                }
            ).AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddMediatR(typeof(HandlerResponse).Assembly);
            services.AddTransient<IMailService, MailService>();
            services.AddSeeTrue();
            services.AddHttpContextAccessor();

            // services.AddFluentValidation();
            // services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PersonValidator>());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeeTrue", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Environment.GetEnvironmentVariable

            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
