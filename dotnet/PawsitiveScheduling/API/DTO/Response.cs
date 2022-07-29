using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
