using Autofac.Extras.Moq;
using Moq;
using PawsitiveScheduling.API.Appointments;
using PawsitiveScheduling.API.Appointments.DTO;
using PawsitiveScheduling.API.DTO;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Database;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PawsitiveScheduling.Tests.API.Appointments
{
    [Collection("json")]
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

            dbUtility.Setup(x => x.AddEntity(It.IsAny<Appointment>())).ReturnsAsync((Appointment appointment) =>
            {
                appointment.Id = "appointmentId";
                return appointment;
            });

            dbUtility.Setup(x => x.GetTracker()).ReturnsAsync(new Tracker());
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

        [Fact]
        public async Task Handle_SavesAppointment()
        {
            var request = new AddAppointmentRequest
            {
                Duration = 10,
                AutoAssigned = false,
                GroomerId = "groomer1",
                StartDate = DateTime.Now,
            };

            var testObject = mock.Create<AddAppointmentHandler>();

            await testObject.Handle(request);

            dbUtility.Verify(x => x.AddEntity(It.Is<Appointment>(y => ValidateAppointment(request, y))), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdatesTracker_WhenAppointmentWasAutoAssigned()
        {
            var request = new AddAppointmentRequest
            {
                Duration = 5,
                AutoAssigned = true,
                GroomerId = "groomer12",
                StartDate = DateTime.Now,
            };

            var testObject = mock.Create<AddAppointmentHandler>();

            await testObject.Handle(request);

            dbUtility.Verify(x => x.UpdateEntity(It.Is<Tracker>(y => y.LastAutoAssignedGroomerId == request.GroomerId)), Times.Once);
        }

        [Fact]
        public async Task Handle_ReturnsAppointmentId()
        {
            var request = new AddAppointmentRequest
            {
                Duration = 6,
                AutoAssigned = false,
                GroomerId = "groomer3",
                StartDate = DateTime.Now,
            };

            var testObject = mock.Create<AddAppointmentHandler>();

            var result = (Response) await testObject.Handle(request);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("appointmentId", result.GetProperty<string>("id"));
        }

        private bool ValidateAppointment(AddAppointmentRequest request, Appointment savedAppointment)
        {
            Assert.Equal(request.StartDate, savedAppointment.ScheduledTime.Start);
            Assert.Equal(TimeSpan.FromMinutes(request.Duration), savedAppointment.ScheduledTime.Duration);
            Assert.Equal(request.GroomerId, savedAppointment.GroomerId);

            return true;
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
