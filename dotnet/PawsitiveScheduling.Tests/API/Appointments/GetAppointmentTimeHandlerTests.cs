using Autofac.Extras.Moq;
using Moq;
using PawsitiveScheduling.API.Appointments;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.API.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using PawsitivityScheduler.Entities.Dogs;
using PawsitivityScheduler.Entities.Dogs.Breeds;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PawsitiveScheduling.Tests.API.Appointments
{
    [Collection("json")]
    public class GetAppointmentTimeHandlerTests
    {
        private readonly AutoMock mock;
        private readonly Mock<IDatabaseUtility> dbUtility;
        private readonly Mock<ILog> log;

        public GetAppointmentTimeHandlerTests()
        {
            mock = AutoMock.GetLoose();
            dbUtility = mock.Mock<IDatabaseUtility>();
            log = mock.Mock<ILog>();
        }

        [Theory]
        [MemberData(nameof(GetInvalidRequests))]
        public async Task Handle_Returns400_IfRequestIsInvalid(GetAppointmentTimeRequest request, string propertyName)
        {
            var testObject = mock.Create<GetAppointmentTimeHandler>();

            var result = (Response) await testObject.Handle(request);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.True((result.Body as ErrorResponse)?.Errors?.FindIndex(x => x.Contains(propertyName, StringComparison.OrdinalIgnoreCase)) != -1);
        }

        [Fact]
        public async Task Handle_Returns404_IfGroomerIsNotFound()
        {
            dbUtility.Setup(x => x.GetEntity<Groomer>(It.IsAny<string>())).ReturnsAsync((Groomer?) null);

            var testObject = mock.Create<GetAppointmentTimeHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentTimeRequest
            {
                DogId = "test",
                GroomerId = "test"
            });

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Returns404_IfServiceIsNotFound()
        {
            dbUtility.Setup(x => x.GetEntity<Service>(It.IsAny<string>())).ReturnsAsync((Service?) null);

            var testObject = mock.Create<GetAppointmentTimeHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentTimeRequest
            {
                DogId = "test",
                GroomerId = "test"
            });

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Returns404_IfDogIsNotFound()
        {
            dbUtility.Setup(x => x.GetEntity<Dog>(It.IsAny<string>())).ReturnsAsync((Dog?) null);

            var testObject = mock.Create<GetAppointmentTimeHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentTimeRequest
            {
                DogId = "test",
                GroomerId = "test"
            });

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Returns404_IfBreedIsNotFound()
        {
            dbUtility.Setup(x => x.GetBreedByName(It.IsAny<BreedName>())).ReturnsAsync((Breed?) null);

            var testObject = mock.Create<GetAppointmentTimeHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentTimeRequest
            {
                DogId = "test",
                GroomerId = "test"
            });

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        // TODO: Add time calculation tests when finalized

        private static IEnumerable<object[]> GetInvalidRequests()
        {
            yield return new object[]
            {
                // Missing DogId
                new GetAppointmentTimeRequest
                {
                    GroomerId = "12"
                },
                "DogId",
            };

            yield return new object[]
            {
                // Missing GroomerId
                new GetAppointmentTimeRequest
                {
                    DogId = "12"
                },
                "GroomerId",
            };
        }
    }
}
