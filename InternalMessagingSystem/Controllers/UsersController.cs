using System.ComponentModel.DataAnnotations;
using InternalMessagingSystem.Models;
using InternalMessagingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalMessagingSystem.Controllers
{
    /// <summary>
    /// Controller for managing users in the internal messaging system.
    /// </summary>
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">The user service used for user operations.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="userName">The username of the user to add.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUserAsync([Required] string userName)
        {
            var user = await _userService.CreateUserAsync(userName);
            return CreatedAtAction("GetUser", new { userId = user.Id }, user);
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>An <see cref="ActionResult"/> containing a list of users.</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            var users = await _userService.GetUsersAsync();
            return users.ToList();
        }

        /// <summary>
        /// Get a user.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <returns>An <see cref="ActionResult"/> containing a list of users.</returns>
        /// <response code="200">Success</response>
        /// <response code="404">User Not Found</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserAsync(Guid userId)
        {
            var user = await _userService.GetUserAsync(userId);
            return user;
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userId">The Id of the user to delete.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="204">Success</response>
        /// <response code="404">User Not Found</response>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserAsync(Guid userId)
        {
            await _userService.DeleteUserAsync(userId);
            return NoContent();
        }
    }
}
