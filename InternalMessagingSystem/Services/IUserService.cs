using InternalMessagingSystem.Models;
using InternalMessagingSystem.Exceptions;

namespace InternalMessagingSystem.Services
{
    /// <summary>
    /// Interface for the user service.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The created user.</returns>
        Task<User> CreateUserAsync(string name);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        Task DeleteUserAsync(Guid userId);

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <exception cref="UserNotFoundException">Thrown when the user with the specified ID is not found.</exception>
        Task<User> GetUserAsync(Guid userId);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        Task<List<User>> GetUsersAsync();
    }
}
