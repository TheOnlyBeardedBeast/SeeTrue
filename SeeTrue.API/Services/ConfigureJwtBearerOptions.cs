using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SeeTrue.API.Services
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IMemoryCache cache;

        public ConfigureJwtBearerOptions(IMemoryCache cache)
        {
            this.cache = cache;
            Console.WriteLine("Joo I am inited");
        }

        public void PostConfigure(string name, JwtBearerOptions opt)
        {

            opt.SaveToken = true;

            opt.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://localhost:5000/",
                // ValidAudience = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 })
            };

            opt.Events = new JwtBearerEvents
            {
                OnTokenValidated = async (TokenValidatedContext context) =>
                {
                    var hasLid = context.Principal.HasClaim(e => e.Type == "lid");

                    if (!hasLid)
                    {
                        context.Fail("Invalid token");
                    }

                    //TODO: check if cache has the lid, if the cache contains lid then it is blaclisted and we have to Fail the token validation
                    
                }
            };
        }
    }
}
