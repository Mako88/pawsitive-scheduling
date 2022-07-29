﻿using PawsitiveScheduling.API.Auth.DTO;
using PawsitiveScheduling.Utility;
using PawsitiveScheduling.Utility.Auth;
using PawsitiveScheduling.Utility.Database;
using PawsitiveScheduling.Utility.DI;
using System.Data;
using System.Threading.Tasks;

namespace PawsitiveScheduling.Entities.Users
{
    /// <summary>
    /// Utililty for managing users
    /// </summary>
    [Component]
    public class UserUtility : IUserUtility
    {
        private readonly IHashingUtility hashingUtility;
        private readonly IDatabaseUtility dbUtility;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public UserUtility(IHashingUtility hashingUtility, IDatabaseUtility dbUtility, ILog log)
        {
            this.hashingUtility = hashingUtility;
            this.dbUtility = dbUtility;
            this.log = log;
        }

        /// <summary>
        /// Create a user of the proper type
        /// </summary>
        public async Task<User> CreateUser(RegisterUserRequest request)
        {
            var existingUser = await dbUtility.GetUserByEmail(request.Email);

            if (existingUser != null)
            {
                throw new DuplicateNameException($"A user with the email '{request.Email}' already exists");
            }

            log.Info($"Creating user '{request.Email}'");

            var user = request.Role switch
            {
                _ => new User(),
            };

            user.Email = request.Email;
            user.Role = request.Role;

            user.Password = hashingUtility.CreateHash(request.Password);

            user = await SaveUser(user);

            log.Info("User saved");

            return user;
        }

        /// <summary>
        /// Save a user as the proper type
        /// </summary>
        private async Task<User> SaveUser(User user) =>
            user.Role switch
            {
                _ => await dbUtility.AddEntity(user),
            };
    }
}
