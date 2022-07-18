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
    public interface IAppointmentRepository : IDatabaseUtility
    {
        /// <summary>
        /// Get all appointments that intersect the given start and end dates
        /// </summary>
        Task<IEnumerable<Appointment>> GetAppointments(DateTime startDate, DateTime endDate);
    }
}