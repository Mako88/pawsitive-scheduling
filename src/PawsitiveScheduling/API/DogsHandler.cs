using Microsoft.AspNetCore.Mvc;
using PawsitiveScheduling.Utility;
using PawsitivityScheduler.Data;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API
{
    /// <summary>
    /// Handler for api/dogs/* endpoints
    /// </summary>
    [Route("api/dogs")]
    [ApiController]
    public class DogsHandler : ControllerBase
    {
        private readonly IDatabaseUtility dbUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public DogsHandler(IDatabaseUtility dbUtility)
        {
            this.dbUtility = dbUtility;
        }

        /// <summary>
        /// Add a new dog
        /// </summary>
        [HttpPost]
        [Route("add")]
        public async Task<string> AddDog(Dog dog)
        {
            var savedDog = await dbUtility.AddDog(dog).ConfigureAwait(false);

            return savedDog.Id;
        }

        /// <summary>
        /// Get a dog by ID
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<Dog> GetDog(string id) =>
            await dbUtility.GetDog(id).ConfigureAwait(false);
    }
}
