using System.Collections.Concurrent;
using InternalMessagingSystem.Exceptions;
using InternalMessagingSystem.Models;

namespace InternalMessagingSystem.Services
{
    /// <summary>
    /// Implementation of the message service using a concurrent dictionary for storage.
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly ConcurrentDictionary<Guid, Message> _messages;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="userService">The user service used for user-related operations.</param>
        public MessageService(IUserService userService)
        {
            _messages = new ConcurrentDictionary<Guid, Message>();
            _userService = userService;
        }

        /// <summary>
        /// Sends a message asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user sending the message.</param>
        /// <param name="messageDTO">The message data transfer object.</param>
        /// <returns>The sent message.</returns>
        public async Task<Message> SendMessageAsync(Guid userId, CreateMessageDTO messageDTO)
        {
            // Ensure the user exists
            await _userService.GetUserAsync(userId);
            await _userService.GetUserAsync(messageDTO.ReceiverId!.Value);

            // Create a new message
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = userId,
                ReceiverId = messageDTO.ReceiverId!.Value,
                Time = DateTime.UtcNow,
                Content = messageDTO.Content,
                IsDeletedBySender = false,
                IsDeletedByReceiver = false
            };

            // Store the message in the dictionary
            _messages[message.Id] = message;

            return await Task.FromResult(message);
        }

        /// <summary>
        /// Retrieves a specific user message asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="messageId">The ID of the message.</param>
        /// <returns>The user message.</returns>
        public async Task<Message> GetUserMessageAsync(Guid userId, Guid messageId)
        {
            // Ensure the user exists
            await _userService.GetUserAsync(userId);

            // Get the message
            var message = await GetMessageAsync(messageId);

            // Check if the user is the sender or receiver of the message and it is not deleted by them
            if (!(message.SenderId == userId && !message.IsDeletedBySender
                || message.ReceiverId == userId && !message.IsDeletedByReceiver))
            {
                throw new MessageNotFoundException($"Message with ID '{messageId}' not found for the user.");
            }

            return await Task.FromResult(message);
        }

        /// <summary>
        /// Retrieves all messages for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>An enumerable collection of user messages.</returns>
        public async Task<IEnumerable<Message>> GetUserMessagesAsync(Guid userId)
        {
            // Ensure the user exists
            await _userService.GetUserAsync(userId);

            // Get the user's messages
            var userMessages = _messages.Values
                .Where(m => m.SenderId == userId && !m.IsDeletedBySender
                    || m.ReceiverId == userId && !m.IsDeletedByReceiver);

            return await Task.FromResult(userMessages);
        }

        /// <summary>
        /// Deletes a specific user message asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="messageId">The ID of the message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteUserMessageAsync(Guid userId, Guid messageId)
        {
            // Get the user message
            var message = await GetUserMessageAsync(userId, messageId);

            // Set the appropriate delete flags based on the user's role
            message.IsDeletedBySender = userId == message.SenderId;
            message.IsDeletedByReceiver = userId == message.ReceiverId;

            // If both sender and receiver have deleted the message, delete it from the dictionary
            if (message.IsDeletedBySender && message.IsDeletedByReceiver)
            {
                await DeleteMessageAsync(messageId);
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Deletes all messages for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteUserMessagesAsync(Guid userId)
        {
            // Ensure the user exists
            await _userService.GetUserAsync(userId);

            // Get the keys of messages sent or received by the user
            var keys = _messages.Keys.Where(m => _messages[m].SenderId == userId
                || _messages[m].ReceiverId == userId).ToList();

            // Delete each user message
            foreach (var key in keys)
            {
                await DeleteUserMessageAsync(userId, _messages[key].Id);
            }

            await Task.CompletedTask;
        }

        private async Task<Message> GetMessageAsync(Guid messageId)
        {
            // Try to get the message from the dictionary
            if (!_messages.TryGetValue(messageId, out var message))
            {
                throw new MessageNotFoundException($"Message with ID '{messageId}' not found for the user.");
            }

            return await Task.FromResult(message);
        }

        private async Task DeleteMessageAsync(Guid messageId)
        {
            // Try to remove the message from the dictionary
            if (!_messages.TryRemove(messageId, out _))
            {
                throw new MessageNotFoundException($"Message with ID '{messageId}' not found for the user.");
            }

            await Task.CompletedTask;
        }
    }
}
