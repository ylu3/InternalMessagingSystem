using System.Collections.Concurrent;
using InternalMessagingSystem.Exceptions;
using InternalMessagingSystem.Models;

namespace InternalMessagingSystem.Services
{
    /// <summary>
    /// Implementation of the user service using a concurrent dictionary for storage.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ConcurrentDictionary<Guid, User> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService()
        {
            _users = new ConcurrentDictionary<Guid, User>();
        }

        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The created user.</returns>
        public async Task<User> CreateUserAsync(string name)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            // Store the user in the dictionary
            _users[user.Id] = user;
            return await Task.FromResult(user);
        }

        /// <summary>
        /// Deletes a user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteUserAsync(Guid userId)
        {
            // Try to remove the user from the dictionary
            if (!_users.TryRemove(userId, out _))
            {
                throw new UserNotFoundException($"User with ID '{userId}' not found.");
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves a user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The retrieved user.</returns>
        public async Task<User> GetUserAsync(Guid userId)
        {
            // Try to get the user from the dictionary
            if (!_users.TryGetValue(userId, out var user))
            {
                throw new UserNotFoundException($"User with ID '{userId}' not found.");
            }

            return await Task.FromResult(user);
        }

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<List<User>> GetUsersAsync()
        {
            // Get all users from the dictionary
            var users = new List<User>(_users.Values);
            return await Task.FromResult(users);
        }
    }
}
