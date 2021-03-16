using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsitivityScheduler.Data;
using System;
using System.Linq;
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
        private readonly SchedulingContext context;

        /// <summary>
        /// Constructor
        /// </summary>
        public DogsHandler(SchedulingContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Add a new dog
        /// </summary>
        [HttpPost]
        [Route("add")]
        public async Task<Guid> AddDog(Dog dog)
        {
            dog.Breed = context.Breeds.FirstOrDefault(x => x.Name == dog.Breed.Name);
            context.Dogs.Add(dog);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return dog.ID;
        }

        [HttpGet]
        [Route("get")]
        public async Task<Dog> GetDog()
        {
            var test = context.Dogs.Include(x => x.Breed).FirstOrDefault();

            return test;
        }
    }
}
