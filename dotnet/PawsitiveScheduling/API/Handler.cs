using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PawsitiveScheduling.Utility;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API
{
    /// <summary>
    /// Base class for handlers
    /// </summary>
    public class Handler : IHandler
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public Handler(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Create an empty 200 response
        /// </summary>
        protected IResult CreateResponse() =>
            new Response();

        /// <summary>
        /// Create a response object with the given status code
        /// </summary>
        protected IResult CreateResponse(HttpStatusCode statusCode) =>
            new Response(statusCode);

        /// <summary>
        /// Create a 200 response with the given body
        /// </summary>
        protected IResult CreateResponse(object body) =>
            new Response(body);

        /// <summary>
        /// Create a response object with the given status code and body
        /// </summary>
        protected IResult CreateResponse(HttpStatusCode statusCode, object body) =>
            new Response(statusCode, body);

        /// <summary>
        /// Create an error response with the given error message
        /// </summary>
        protected IResult CreateResponse(HttpStatusCode statusCode, string errorMessage)
        {
            log.Error(errorMessage);
            return new Response(statusCode, new { Error = errorMessage });
        }

        /// <summary>
        /// Create an error response with the given exception
        /// </summary>
        protected IResult CreateResponse(HttpStatusCode statusCode, Exception ex)
        {
            log.Error(ex.Message);
            return new Response(statusCode, new { Error = ex.Message, Stack = ex.StackTrace });
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public virtual void MapEndpoint(WebApplication app) => throw new System.NotImplementedException("MapEndpoint() was called on the base Handler class");

        /// <summary>
        /// An implementation of IResult to return responses
        /// </summary>
        private class Response : IResult
        {
            public object Body { get; set; }

            public HttpStatusCode? StatusCode { get; set; }

            public Response()
            {

            }

            public Response(object body)
            {
                Body = body;
            }

            public Response(HttpStatusCode statusCode)
            {
                StatusCode = statusCode;
            }

            public Response(HttpStatusCode statusCode, object body)
            {
                Body = body;
                StatusCode = statusCode;
            }

            public async Task ExecuteAsync(HttpContext context)
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = (int) (StatusCode == null ? HttpStatusCode.OK : StatusCode.Value);

                if (Body != null)
                {
                    var json = JsonConvert.SerializeObject(Body);

                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}
