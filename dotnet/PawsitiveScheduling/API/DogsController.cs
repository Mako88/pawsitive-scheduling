using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PawsitiveScheduling.Utility;
using PawsitivityScheduler.Data;
using System.Threading.Tasks;

namespace PawsitiveScheduling.API.Auth
{
    /// <summary>
    /// Controller for api/dogs/* endpoints
    /// </summary>
    [Route("api/dogs")]
    [ApiController]
    [Authorize]
    public class DogsController : ControllerBase
    {
        private readonly IDatabaseUtility dbUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public DogsController(IDatabaseUtility dbUtility)
        {
            this.dbUtility = dbUtility;
        }

        /// <summary>
        /// Add a new dog
        /// </summary>
        [HttpPost]
        [Route("add")]
        public async Task<string> AddDog([FromBody] Dog dog)
        {
            var savedDog = await dbUtility.AddEntity(dog).ConfigureAwait(false);

            return savedDog.Id.ToString();
        }

        /// <summary>
        /// Get a dog by ID
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<Dog> GetDog([FromRoute] string id) =>
            await dbUtility.GetEntity<Dog>(ObjectId.Parse(id)).ConfigureAwait(false);
    }
}
