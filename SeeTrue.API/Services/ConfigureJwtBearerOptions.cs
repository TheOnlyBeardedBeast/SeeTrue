using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SeeTrue.Infrastructure.Extensions;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.API.Services
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IMemoryCache cache;

        public ConfigureJwtBearerOptions(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void PostConfigure(string name, JwtBearerOptions opt)
        {

            opt.SaveToken = true;

            opt.TokenValidationParameters = new()
            {
                ValidateIssuer = bool.Parse(Environment.GetEnvironmentVariable("SEETRUE_VALIDATE_ISSUER")),
                ValidateAudience = bool.Parse(Environment.GetEnvironmentVariable("SEETRUE_VALIDATE_AUDIENCE")),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("SEETRUE_ISSUER"),
                ValidAudiences = Helpers.ParseAudiences(Environment.GetEnvironmentVariable("SEETRUE_AUIDIENCES")),
                IssuerSigningKey = new SymmetricSecurityKey(Environment.GetEnvironmentVariable("SEETRUE_SIGNING_KEY").ToByteArray())
            };

            opt.Events = new JwtBearerEvents
            {
                OnTokenValidated = (TokenValidatedContext context) =>
                {
                    Predicate<Claim> check = e => e.Type == "lid";

                    var hasLid = context.Principal.HasClaim(check);

                    if (!hasLid)
                    {
                        context.Fail("Invalid token");
                    }

                    var lid = context.Principal.Claims.FirstOrDefault(e => e.Type == "lid").Value;

                    if (this.cache.TryGetValue(lid, out var _))
                    {
                        context.Fail("Invalid token");
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }
}
