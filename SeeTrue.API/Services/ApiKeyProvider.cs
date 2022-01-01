using System;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.API.Services
{
    public class ApiKeyProvider : IApiKeyProvider
    {
        public Task<IApiKey> ProvideAsync(string key)
        {
            if (Env.ApiKey.Key == key)
            {
                return Task.FromResult(Env.ApiKey);
            }

            return null;
        }
    }
}
