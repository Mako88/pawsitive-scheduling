using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.DTO
{
    /// <summary>
    /// An implementation of IResult to return responses
    /// </summary>
    public class Response : IResult
    {
        public object? Body { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public Response()
        {
            StatusCode = HttpStatusCode.OK;
        }

        public Response(object body)
        {
            Body = body;
            StatusCode = HttpStatusCode.OK;
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

        /// <summary>
        /// Implement IResult to write the response
        /// </summary>
        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int) StatusCode;

            if (Body != null)
            {
                var json = JsonConvert.SerializeObject(Body);

                await context.Response.WriteAsync(json);
            }
        }

        /// <summary>
        /// For tests to get the json of a response
        /// </summary>
        internal string GetJson() => JsonConvert.SerializeObject(Body);

        /// <summary>
        /// For tests to get a property from the body of a response
        /// </summary>
        internal T? GetProperty<T>(string propertyName) where T : class =>
            JToken.Parse(GetJson())?[propertyName]?.ToObject<T>();
    }
}
