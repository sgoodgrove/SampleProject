using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a list of all users stored in the system
        /// </summary>
        /// <returns>Every user stored in the system</returns>
        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            /* The underlying userRepository.GetAllUsers() uses Task.Run(), so isn't strictly an asynchronous I/O
             * operation and is threaded, so there isn't necessarily a benefit in making this action asynchronous, it
             * does however demonstrate some awareness of async operations. */

            try
            {
                return await userRepository.GetAllUsers();
            }
            catch (Exception)
            {
                /* This try/catch is fairly common here, in production I would consider making this a filter rather
                 * than repeating the code. */
                Response.StatusCode = 400;
                return null;
            }
        }

        /// <summary>
        /// Returns an individual User by a given <paramref name="id"/>
        /// </summary>
        /// <param name="id">Unique idenfier of a user</param>
        /// <returns>All user details for request user</returns>
        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public User Get(int id)
        {
            try
            {
                return userRepository.GetUser(id);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
                return null;
            }
        }

        // POST: api/Users
        /// <summary>
        /// Create a new user. Username must be unique
        /// </summary>
        /// <param name="value">The User object to create</param>
        /// <returns>The ID of the created record</returns>
        [HttpPost]
        public int Post([FromBody]User value)
        {
            try
            {
                return userRepository.CreateUser(value);
            }
            catch (Exception)
            {
                Response.StatusCode = 400;
                return 0;
            }
        }

        /// <summary>
        /// Updates user details for specified <paramref name="user"/>. Fields that are NULL are not updated.
        /// </summary>
        /// <param name="id">The user ID of the object to update</param>
        /// <param name="user">The User object containing updated user details.</param>
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, User user)
        {
            try
            {
                userRepository.UpdateUser(user);
            }
            catch (InvalidOperationException)
            {
                Response.StatusCode = 404;
            }
        }

        /// <summary>
        /// Remove a user record from the system
        /// </summary>
        /// <param name="id">The user ID of the object to remove</param>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            userRepository.DeleteUser(id);
        }
    }
}
