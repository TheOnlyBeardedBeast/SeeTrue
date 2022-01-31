using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SeeTrue.API.DB;
using SeeTrue.Infrastructure.Types;

namespace SeeTrue.API.Filters
{
    public class SeeTrueExceptionFilter : IExceptionFilter
    {
        private readonly AppDbContext dbContext;

        public SeeTrueExceptionFilter(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void OnException(ExceptionContext context)
        {
            dbContext.RollbackTransaction();

            if (context.Exception is SeeTrueException exception)
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int)exception.StatusCode,
                    Content = exception.Message
                };
            }
            else
            {
                context.Result = new ContentResult
                {
                    StatusCode = 500,
                    Content = context.Exception.Message
                };
            }


        }
    }
}
