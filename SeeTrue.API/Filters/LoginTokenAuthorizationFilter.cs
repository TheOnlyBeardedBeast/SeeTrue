using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace SeeTrue.API.Filters
{
    public class LoginTokenAuthorizationFilter : IActionFilter
    {
        private readonly IMemoryCache cache;

        public LoginTokenAuthorizationFilter(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
        }

        

        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
            
        //    Console.WriteLine("############################### was here");
        //    Console.WriteLine(context is null);
        //    context.HttpContext.User
        //    Console.WriteLine(context.HttpContext is null);
        //    Console.WriteLine(context.HttpContext.User is null);
        //    Console.WriteLine(context.HttpContext.User.Claims is null);
        //    //Console.WriteLine(JsonSerializer.Serialize(context.HttpContext.User.Claims.FirstOrDefault(e => e.Type == "lid")));
        //    var lid = context.HttpContext.User.HasClaim(e => e.Type == "lid");
        //    if (!lid)
        //    {
        //        context.Result = new UnauthorizedResult();
        //    }
        //    return;
        //    //if (cache.TryGetValue(lid, out var val))
        //    //{
        //    //    return;
        //    //}

        //    context.Result = new UnauthorizedResult();
        //}
    }
}
