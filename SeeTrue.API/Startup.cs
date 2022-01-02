using System.Security.Claims;
using System.Linq;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SeeTrue.API.Db;
using SeeTrue.API.Filters;
using SeeTrue.API.Services;
using SeeTrue.Infrastructure;
using SeeTrue.Models;
using SeeTrue.Utils.Services;
using SeeTrue.Infrastructure.Utils;
using AspNetCore.Authentication.ApiKey;

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
            var type = typeof(Env);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

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

            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3001");
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                      builder.AllowCredentials();
                                  });
            });

            services.AddAuthorization(options =>
            {
                if (Env.AdminRole is not null)
                {
                    options.AddPolicy("Admin", policy =>
                    {
                        policy.RequireAssertion(context =>
                        {
                            var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                            return role is not null && Env.AdminRole is not null && role == Env.AdminRole;
                        });
                        policy.RequireAuthenticatedUser();
                        policy.AddAuthenticationSchemes(ApiKeyDefaults.AuthenticationScheme);
                        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    });
                }
                else
                {
                    options.AddPolicy("Admin", policy =>
                    {
                        policy.AddAuthenticationSchemes(ApiKeyDefaults.AuthenticationScheme);
                        policy.RequireAuthenticatedUser();

                    });
                }

            }).AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer().AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
            {
                options.Realm = "Sample Web API";
                options.KeyName = "X-API-KEY";
            });

            services.AddMediatR(typeof(ServiceExtension).Assembly);
            services.AddTransient<IMailService, MailService>();
            services.AddSeeTrue();
            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeeTrue", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();
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

            app.UseRouting();
            app.UseCors("cors");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
