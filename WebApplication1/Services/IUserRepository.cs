using System.Collections.Generic;
using System.Threading.Tasks;

using WebApplication1.Models;

namespace WebApplication1.Services
{
    /// <summary>
    /// Stores and queries the underlying data store for User objects.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Create a new user in the underlying data store.
        /// </summary>
        /// <param name="user">The user object you wish to store.</param>
        /// <returns>A new User object, which includes the updated User ID.</returns>
        int CreateUser(User user);

        /// <summary>
        /// Removes a user from the underlying data store.
        /// </summary>
        /// <param name="userId">The ID of the user record you wish to remove.</param>
        void DeleteUser(int userId);

        /// <summary>
        /// Retrieve a user from the underlying data store based on user ID.
        /// </summary>
        /// <param name="userId">The ID of the user record you with to retrieve.</param>
        /// <returns>A User object containing all user details for the requested user.</returns>
        User GetUser(int userId);

        /// <summary>
        /// A full list of all Users stored in the underlying data store.
        /// </summary>
        /// <returns>A full list of all Users stored in the underlying data store.</returns>
        Task<IEnumerable<User>> GetAllUsers();

        /// <summary>
        /// Updates a User record in the underlying data store. NULL field values will cause no update to exist fields.
        /// </summary>
        /// <param name="user">The user object containing updated user details.</param>
        void UpdateUser(User user);
    }
}
