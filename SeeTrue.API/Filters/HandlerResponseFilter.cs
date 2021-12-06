using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeeTrue.CQRS;

namespace SeeTrue.API.Filters
{
    public class HandlerResponseFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Filter");

            if (context.Result is ObjectResult objectResult && objectResult.Value is IHandlerResponse iHandlerResponse)
            {
                var handlerResponse = (HandlerResponse)objectResult.Value;
                Console.WriteLine(handlerResponse.Code);
                //switch (handlerResponse.Code)
                //{
                //    case HttpStatusCode.OK:
                //        {
                //            Console.WriteLine("OK");
                //            context.Result = new ObjectResult(new { handlerResponse.Data }) { StatusCode = (int)handlerResponse.Code };
                //            break;
                //        }
                //    case HttpStatusCode.NoContent:
                //        {
                //            Console.WriteLine("NOC");
                //            context.Result = new ObjectResult(null) { StatusCode = (int)handlerResponse.Code };
                //            break;
                //        }
                //    default:
                //        {
                //            Console.WriteLine("NOK");
                //            context.Result = new ObjectResult(new { handlerResponse.ErrorMessage }) { StatusCode = (int)handlerResponse.Code };
                //            break;
                //        }
                //}

            } 
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}
