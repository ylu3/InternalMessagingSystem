using InternalMessagingSystem.Models;

namespace InternalMessagingSystem.Services
{
    /// <summary>
    /// Interface for the message service.
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Sends a message asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user sending the message.</param>
        /// <param name="messageDTO">The message data transfer object.</param>
        /// <returns>The sent message.</returns>
        public Task<Message> SendMessageAsync(Guid userId, CreateMessageDTO messageDTO);

        /// <summary>
        /// Retrieves all messages for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An enumerable collection of user messages.</returns>
        public Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId);

        /// <summary>
        /// Retrieves a specific user message asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="messageId">The ID of the message.</param>
        /// <returns>The user message.</returns>
        public Task<Message> GetUserMessageAsync(Guid userId, Guid messageId);

        /// <summary>
        /// Deletes all messages for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteUserMessagesAsync(Guid userId);

        /// <summary>
        /// Deletes a specific user message asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="messageId">The ID of the message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task DeleteUserMessageAsync(Guid userId, Guid messageId);
    }
}
