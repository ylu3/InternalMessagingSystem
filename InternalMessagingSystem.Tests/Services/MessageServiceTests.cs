using InternalMessagingSystem.Exceptions;
using InternalMessagingSystem.Models;
using InternalMessagingSystem.Services;
using Moq;

namespace InternalMessagingSystem.Tests
{
    public class MessageServiceTests
    {
        private readonly MessageService _messageService;
        private readonly Mock<IUserService> _userServiceMock;

        public MessageServiceTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _messageService = new MessageService(_userServiceMock.Object);
        }

        [Fact]
        public async Task SendMessageAsync_ValidMessage_ReturnsSentMessage()
        {
            // ARRANGE
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var messageDto = new CreateMessageDTO { ReceiverId = receiverId, Content = "Hello, World!" };
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));

            // ACT
            var message = await _messageService.SendMessageAsync(senderId, messageDto);

            // ASSERT
            Assert.NotNull(message);
            Assert.Equal(senderId, message.SenderId);
            Assert.Equal(receiverId, message.ReceiverId);
            Assert.Equal(messageDto.Content, message.Content);
        }

        [Fact]
        public async Task SendMessageAsync_NonExistentUser_ThrowsUserNotFoundException()
        {
            // ARRANGE
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var messageDto = new CreateMessageDTO { ReceiverId = receiverId, Content = "Hello, World!" };
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Throws(new UserNotFoundException("User not found"));

            // ACT & ASSERT
            await Assert.ThrowsAsync<UserNotFoundException>(() => _messageService.SendMessageAsync(senderId, messageDto));
        }

        [Fact]
        public async Task GetUserMessageAsync_ValidMessage_ReturnsUserMessage()
        {
            // ARRANGE
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var messageDto = new CreateMessageDTO { ReceiverId = receiverId, Content = "Hello, World!" };
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            var message = await _messageService.SendMessageAsync(senderId, messageDto);

            // ACT
            var fetchedMessage = await _messageService.GetUserMessageAsync(senderId, message.Id);

            // ASSERT
            Assert.NotNull(fetchedMessage);
            Assert.Equal(senderId, fetchedMessage.SenderId);
            Assert.Equal(receiverId, fetchedMessage.ReceiverId);
            Assert.Equal(messageDto.Content, fetchedMessage.Content);
        }

        [Fact]
        public async Task GetUserMessageAsync_NonExistentMessage_ThrowsMessageNotFoundException()
        {
            // ARRANGE
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));

            // ACT & ASSERT
            await Assert.ThrowsAsync<MessageNotFoundException>(() => _messageService.GetUserMessageAsync(userId, messageId));
        }

        [Fact]
        public async Task GetUserMessagesAsync_MultipleMessages_ReturnsUserMessages()
        {
            // ARRANGE
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var messageDto = new CreateMessageDTO { ReceiverId = receiverId, Content = "Hello, World!" };
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            var message1 = await _messageService.SendMessageAsync(senderId, messageDto);
            var message2 = await _messageService.SendMessageAsync(senderId, messageDto);

            // ACT
            var messages = await _messageService.GetUserMessagesAsync(senderId);

            // ASSERT
            Assert.Contains(message1, messages);
            Assert.Contains(message2, messages);
        }

        [Fact]
        public async Task DeleteUserMessageAsync_ValidMessage_RemovesUserMessage()
        {
            // ARRANGE
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var messageDto = new CreateMessageDTO { ReceiverId = receiverId, Content = "Hello, World!" };
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            var message = await _messageService.SendMessageAsync(senderId, messageDto);

            // ACT
            await _messageService.DeleteUserMessageAsync(senderId, message.Id);

            // ASSERT
            await Assert.ThrowsAsync<MessageNotFoundException>(() => _messageService.GetUserMessageAsync(senderId, message.Id));
        }

        [Fact]
        public async Task DeleteUserMessageAsync_NonExistentMessage_ThrowsMessageNotFoundException()
        {
            // ARRANGE
            var userId = Guid.NewGuid();
            var messageId = Guid.NewGuid();
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));

            // ACT & ASSERT
            await Assert.ThrowsAsync<MessageNotFoundException>(() => _messageService.DeleteUserMessageAsync(userId, messageId));
        }

        [Fact]
        public async Task DeleteUserMessagesAsync_MultipleMessages_RemovesAllUserMessages()
        {
            // ARRANGE
            var senderId = Guid.NewGuid();
            var receiverId = Guid.NewGuid();
            var messageDto = new CreateMessageDTO { ReceiverId = receiverId, Content = "Hello, World!" };
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new User()));
            var message1 = await _messageService.SendMessageAsync(senderId, messageDto);
            var message2 = await _messageService.SendMessageAsync(senderId, messageDto);

            // ACT
            await _messageService.DeleteUserMessagesAsync(senderId);

            // ASSERT
            await Assert.ThrowsAsync<MessageNotFoundException>(() => _messageService.GetUserMessageAsync(senderId, message1.Id));
            await Assert.ThrowsAsync<MessageNotFoundException>(() => _messageService.GetUserMessageAsync(senderId, message2.Id));
        }

        [Fact]
        public async Task DeleteUserMessagesAsync_NonExistentUser_ThrowsUserNotFoundException()
        {
            // ARRANGE
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(service => service.GetUserAsync(It.IsAny<Guid>())).Throws(new UserNotFoundException("User not found"));

            // ACT & ASSERT
            await Assert.ThrowsAsync<UserNotFoundException>(() => _messageService.DeleteUserMessagesAsync(userId));
        }
    }
}
