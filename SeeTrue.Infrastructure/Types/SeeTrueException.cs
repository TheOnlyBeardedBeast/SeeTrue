using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeeTrue.Infrastructure.Types
{
    public class SeeTrueException : Exception
    {
        public HttpStatusCode StatusCode { get; init; }

        public SeeTrueException(HttpStatusCode statusCode, string message = null) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }

    public record SeeTrueExceptionResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; init; }

        public SeeTrueExceptionResponse(SeeTrueException exception)
        {
            this.Message = exception.Message;
        }

        public SeeTrueExceptionResponse(string message)
        {
            this.Message = message;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
