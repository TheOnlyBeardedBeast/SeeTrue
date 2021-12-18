using System;
using System.Net;

namespace SeeTrue.Infrastructure.Types
{
    public class SeeTrueException : Exception
    {
        public HttpStatusCode StatusCode { get; init; }

        public SeeTrueException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
