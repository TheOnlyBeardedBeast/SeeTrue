using System;
using System.Net;

namespace SeeTrue.CQRS
{
    public interface IHandlerResponse { }

    public record HandlerResponse : HandlerResponse<object>
    {
    }

    public record HandlerResponse<T> : IHandlerResponse
    {
        public HttpStatusCode Code { get; init; } = HttpStatusCode.OK;
        public string ErrorMessage { get; init; }
        public T Data { get; init; }
    }
}
