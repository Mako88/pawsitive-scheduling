using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PawsitiveScheduling.API.DTO;
using PawsitiveScheduling.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace PawsitiveScheduling.API
{
    /// <summary>
    /// Base class for handlers
    /// </summary>
    public abstract class Handler : IHandler
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
            return new Response(statusCode, new ErrorResponse { Error = errorMessage });
        }

        /// <summary>
        /// Create an error response with the given exception
        /// </summary>
        protected IResult CreateResponse(HttpStatusCode statusCode, Exception ex)
        {
            log.Error(ex.Message);
            return new Response(statusCode, new ErrorResponse { Error = ex.Message, Stack = ex.StackTrace });
        }

        /// <summary>
        /// Create an error response with the given error messages
        /// </summary>
        protected IResult CreateResponse(HttpStatusCode statusCode, string errorMessage, List<string> errors)
        {
            log.Error($"{errorMessage}:\n{string.Join("\n", errors)}");
            return new Response(statusCode, new ErrorResponse { Error = errorMessage, Errors = errors });
        }

        /// <summary>
        /// Validate a request
        /// </summary>
        protected bool ValidateRequest(object request, out IResult? response)
        {
            var errors = new List<string>();

            foreach (var property in request.GetType().GetProperties())
            {
                foreach (var attribute in property.GetCustomAttributes(typeof(ValidationAttribute), true))
                {
                    var propertyValue = property.CanRead ? property.GetValue(request) : null;

                    var validationAttribute = (ValidationAttribute) attribute;

                    if (!validationAttribute.IsValid(propertyValue))
                    {
                        errors.Add(validationAttribute.FormatErrorMessage(property.Name));
                    }

                    /*
                    var requiredAttribute = attribute as RequiredAttribute;

                    // Add an error for value types that are their default value but are required
                    if (requiredAttribute != null && property.PropertyType.IsValueType && propertyValue != null)
                    {
                        var instance = Activator.CreateInstance(propertyValue.GetType());
                        if (instance == null && instance.Equals(propertyValue))
                        {
                            errors.Add(validationAttribute.FormatErrorMessage(property.Name));
                        }
                    } */
                }
            }

            if (errors.Any())
            {
                response = CreateResponse(HttpStatusCode.BadRequest, "Some validation errors occurred", errors);
                return false;
            }

            response = null;
            return true;
        }

        /// <summary>
        /// Map this handler to an endpoint
        /// </summary>
        public abstract void MapEndpoint(WebApplication app);
    }
}
