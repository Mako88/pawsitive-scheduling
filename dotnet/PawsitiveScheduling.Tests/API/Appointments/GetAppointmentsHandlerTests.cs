using Autofac.Extras.Moq;
using Moq;
using Neleus.LambdaCompare;
using PawsitiveScheduling.API.Appointments;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.API.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Entities.Users;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PawsitiveScheduling.Tests.API.Appointments
{
    [Collection("json")]
    public class GetAppointmentsHandlerTests
    {
        private readonly AutoMock mock;
        private readonly Mock<IDatabaseUtility> dbUtility;
        private readonly Mock<ILog> log;

        public GetAppointmentsHandlerTests()
        {
            mock = AutoMock.GetLoose();
            dbUtility = mock.Mock<IDatabaseUtility>();
            log = mock.Mock<ILog>();

            dbUtility.Setup(x => x.GetTracker()).ReturnsAsync(new Tracker());
        }

        [Fact]
        public async Task Handle_Returns400_IfRequestIsInvalid()
        {
            var request = new GetAppointmentsRequest();

            var testObject = mock.Create<GetAppointmentsHandler>();

            var result = (Response) await testObject.Handle(request);

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.True((result.Body as ErrorResponse)?.Errors?.FindIndex(x => x.Contains("GroomerId", StringComparison.OrdinalIgnoreCase)) != -1);
        }

        [Theory]
        [MemberData(nameof(GetGroomers))]
        public async Task Handle_GetsNextGroomer_IfGroomerIdIsAny(List<Groomer> groomers, string lastGroomerId, string newGroomerId)
        {
            dbUtility.Setup(x => x.GetTracker()).ReturnsAsync(new Tracker
            {
                LastAutoAssignedGroomerId = lastGroomerId
            });

            dbUtility.Setup(x => x.GetEntities(It.IsAny<Expression<Func<Groomer, bool>>>(), It.IsAny<Expression<Func<Groomer, object>>>()))
                .ReturnsAsync(groomers);

            var testObject = mock.Create<GetAppointmentsHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentsRequest
            {
                GroomerId = "any"
            });

            dbUtility.Verify(x => x.UpdateEntity(It.Is<Tracker>(y => y.LastAutoAssignedGroomerId == newGroomerId)), Times.Once);
            dbUtility.Verify(x => x.GetEntities(
                It.Is<Expression<Func<Appointment, bool>>>(y => Lambda.Eq(y, z => z.GroomerId == newGroomerId)),
                It.IsAny<Expression<Func<Appointment, object>>>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Returns422_WhenGroomerIdIsAny_AndNoGroomersExist()
        {
            dbUtility.Setup(x => x.GetEntities(It.IsAny<Expression<Func<Groomer, bool>>>(), It.IsAny<Expression<Func<Groomer, object>>>()))
                .ReturnsAsync(new List<Groomer>());

            var testObject = mock.Create<GetAppointmentsHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentsRequest
            {
                GroomerId = "any"
            });

            Assert.Equal(HttpStatusCode.UnprocessableEntity, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ReturnsAppointments()
        {
            var groomerId = "12";

            dbUtility.Setup(x => x.GetEntities(
                It.Is<Expression<Func<Appointment, bool>>>(y => Lambda.Eq(y, z => z.GroomerId == groomerId)),
                It.IsAny<Expression<Func<Appointment, object>>>()))
                .ReturnsAsync(new List<Appointment>
                {
                    new Appointment
                    {
                        Id = "appointmentId"
                    }
                });

            var testObject = mock.Create<GetAppointmentsHandler>();

            var result = (Response) await testObject.Handle(new GetAppointmentsRequest
            {
                GroomerId = groomerId
            });

            dbUtility.Verify(x => x.GetEntities(
                It.Is<Expression<Func<Appointment, bool>>>(y => Lambda.Eq(y, z => z.GroomerId == groomerId)),
                It.IsAny<Expression<Func<Appointment, object>>>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Contains(result.GetProperty<List<Appointment>>("appointments"), x => x.Id == "appointmentId");
        }

        private static IEnumerable<object[]> GetGroomers()
        {
            yield return new object[]
            {
                new List<Groomer>
                {
                    new Groomer
                    {
                        Id = "1"
                    },
                    new Groomer
                    {
                        Id = "2"
                    },
                    new Groomer
                    {
                        Id = "3"
                    }
                },
                "1",
                "2"
            };

            yield return new object[]
            {
                new List<Groomer>
                {
                    new Groomer
                    {
                        Id = "1"
                    },
                    new Groomer
                    {
                        Id = "2"
                    },
                    new Groomer
                    {
                        Id = "3"
                    }
                },
                "3",
                "1",
            };

            yield return new object[]
            {
                new List<Groomer>
                {
                    new Groomer
                    {
                        Id = "1"
                    },
                    new Groomer
                    {
                        Id = "2"
                    },
                    new Groomer
                    {
                        Id = "3"
                    }
                },
                "2",
                "3",
            };
        }
    }
}
