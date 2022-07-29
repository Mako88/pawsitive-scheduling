using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PawsitiveScheduling.API.Appointments;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.API.DTO;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PawsitiveScheduling.Tests.Appointments
{
    public class AddAppointmentHandlerTests
    {
        private readonly AutoMock mock;
        private readonly Mock<IDatabaseUtility> dbUtility;
        private readonly Mock<ILog> log;

        public AddAppointmentHandlerTests()
        {
            mock = AutoMock.GetLoose();
            dbUtility = mock.Mock<IDatabaseUtility>();
            log = mock.Mock<ILog>();
        }

        [Theory]
        [MemberData(nameof(GetInvalidRequests))]
        public async Task Handle_Returns400_IfRequestIsInvalid(AddAppointmentRequest request, string propertyName)
        {
            var testObject = mock.Create<AddAppointmentHandler>();

            var result = (Response) await testObject.Handle(request);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.True((result.Body as ErrorResponse)?.Errors?.FindIndex(x => x.Contains(propertyName, StringComparison.OrdinalIgnoreCase)) != -1);
        }

        private static async Task<T?> GetResponseValue<T>(IResult result)
        {
            var mockHttpContext = new DefaultHttpContext
            {
                // RequestServices needs to be set so the IResult implementation can log.
                RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
                Response =
                {
                    // The default response body is Stream.Null which throws away anything that is written to it.
                    Body = new MemoryStream(),
                },
            };

            await result.ExecuteAsync(mockHttpContext);

            // Reset MemoryStream to start so we can read the response.
            mockHttpContext.Response.Body.Position = 0;
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            return await JsonSerializer.DeserializeAsync<T>(mockHttpContext.Response.Body, jsonOptions);
        }

        private static IEnumerable<object[]> GetInvalidRequests()
        {
            yield return new object[]
            {
                // Missing StartDate
                new AddAppointmentRequest
                {
                    Duration = 5,
                    GroomerId = "12"
                },
                "StartDate",
            };

            yield return new object[]
            {
                // Missing Duration
                new AddAppointmentRequest
                {
                    StartDate = DateTime.Now,
                    GroomerId = "12"
                },
                "Duration",
            };

            yield return new object[]
            {
                // Missing GroomerId
                new AddAppointmentRequest
                {
                    StartDate = DateTime.Now,
                    Duration = 5,
                },
                "GroomerId"
            };
        }
    }
}
