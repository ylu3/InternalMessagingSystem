using InternalMessagingSystem.Models;
using InternalMessagingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternalMessagingSystem.Controllers
{
    /// <summary>
    /// Controller for managing messages in the internal messaging system.
    /// </summary>
    [Route("users")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesController"/> class.
        /// </summary>
        /// <param name="messageService">The message service used for message operations.</param>
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }


        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <param name="messageDTO">The messageDTO to send.</param>
        /// <param name="userId">The id of the sender.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="201">Success</response>
        /// <response code="404">User not found</response>
        [HttpPost("{userId}/messages")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SendMessageAsync(Guid userId, CreateMessageDTO messageDTO)
        {
            var message = await _messageService.SendMessageAsync(userId, messageDTO);
            return CreatedAtAction("GetUserMessage", new { userId, messageId = message.Id }, message);
        }

        /// <summary>
        /// Gets all messages for a specific user.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <returns>An <see cref="ActionResult"/> containing a list of messages.</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Messages not found</response>
        [HttpGet("{userId}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Message>>> GetUserMessagesAsync(Guid userId)
        {
            var messages = await _messageService.GetUserMessagesAsync(userId);
            return messages.ToList();
        }

        /// <summary>
        /// Get a message for a specific user.
        /// </summary>
        /// <param name="userId">The Id of the user to get.</param>
        /// <param name="messageId">The Id of the message to get.</param>
        /// <returns>An <see cref="ActionResult"/> containing a list of messages.</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Messages not found</response>
        [HttpGet("{userId}/messages/{messageId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Message>> GetUserMessageAsync(Guid userId, Guid messageId)
        {
            var message = await _messageService.GetUserMessageAsync(userId, messageId);
            return message;
        }

        /// <summary>
        /// Deletes all messages for a specific user.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="204">Success</response>
        /// <response code="404">Messages not found</response>
        [HttpDelete("{userId}/messages")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserMessagesAsync(Guid userId)
        {
            await _messageService.DeleteUserMessagesAsync(userId);
            return NoContent();
        }

        /// <summary>
        /// Deletes a message for a specific user.
        /// </summary>
        /// <param name="userId">The Id of the user.</param>
        /// <param name="messageId">The Id of the message to get.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        /// <response code="204">Success</response>
        /// <response code="404">Messages not found</response>
        [HttpDelete("{userId}/messages/{messageId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUserMessageAsync(Guid userId, Guid messageId)
        {
            await _messageService.DeleteUserMessageAsync(userId, messageId);
            return NoContent();
        }
    }
}
