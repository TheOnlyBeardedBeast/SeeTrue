using System;
using System.Threading.Tasks;
using AspNetCore.Authentication.ApiKey;
using SeeTrue.Infrastructure.Utils;

namespace SeeTrue.API.Services
{
    public class ApiKeyProvider: IApiKeyProvider
    {
		public async Task<IApiKey> ProvideAsync(string key)
		{
			if(Env.ApiKey.Key == key)
            {
				return Env.ApiKey;
            }

			return null;
		}
	}
}
