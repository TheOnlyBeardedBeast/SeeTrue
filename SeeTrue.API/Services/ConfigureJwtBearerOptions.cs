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
using Microsoft.AspNetCore.Http;

namespace SeeTrue.API.Services
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IMemoryCache cache;
        private readonly IHttpContextAccessor httpContext;

        public ConfigureJwtBearerOptions(IMemoryCache cache, IHttpContextAccessor httpContext)
        {
            this.cache = cache;
            this.httpContext = httpContext;
        }

        public void PostConfigure(string name, JwtBearerOptions opt)
        {
            opt.SaveToken = true;

            opt.TokenValidationParameters = new()
            {
                ValidateIssuer = Env.ValidateIssuer,
                ValidateAudience = Env.ValidateAudience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Env.Issuer,
                ValidAudiences = Env.Audiences,
                IssuerSigningKey = new SymmetricSecurityKey(Env.SigningKey.ToByteArray())
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

                    // var aud = context.Principal.Claims.FirstOrDefault(e => e.Type == "aud").Value;
                    // var referer = httpContext.HttpContext.Request.GetTypedHeaders().Referer.GetLeftPart(UriPartial.Authority);

                    return Task.CompletedTask;
                }
            };
        }
    }
}
