using Itenso.TimePeriod;
using PawsitiveScheduling.Entities;
using PawsitiveScheduling.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Repositories
{
    /// <summary>
    /// Repo for appointments
    /// </summary>
    public class AppointmentRepository : DatabaseUtility, IAppointmentRepository
    {
        /// <summary>
        /// Get all appointments that intersect the given start and end dates
        /// </summary>
        public async Task<IEnumerable<Appointment>> GetAppointments(DateTime startDate, DateTime endDate)
        {
            var range = new TimeRange(startDate, endDate);

            return await GetEntities<Appointment>(x => x.ScheduledTime.IntersectsWith(range));
        }
    }
}
