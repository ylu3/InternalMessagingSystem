using InternalMessagingSystem.Controllers;
using InternalMessagingSystem.Models;
using InternalMessagingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class MessagesControllerTests
{
    private readonly Mock<IMessageService> _messageServiceMock;
    private readonly MessagesController _controller;

    public MessagesControllerTests()
    {
        _messageServiceMock = new Mock<IMessageService>();
        _controller = new MessagesController(_messageServiceMock.Object);
    }

    [Fact]
    public async Task SendMessageAsync_ValidInput_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var messageDto = new CreateMessageDTO { ReceiverId = Guid.NewGuid(), Content = "Hello, World!" };
        var expectedMessage = new Message { Id = Guid.NewGuid() };
        _messageServiceMock.Setup(service => service.SendMessageAsync(userId, messageDto)).ReturnsAsync(expectedMessage);

        // Act
        var result = await _controller.SendMessageAsync(userId, messageDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnedMessage = Assert.IsType<Message>(createdAtActionResult.Value);

        Assert.Equal(expectedMessage.Id, returnedMessage.Id);
        _messageServiceMock.Verify(service => service.SendMessageAsync(userId, messageDto), Times.Once);
    }

    [Fact]
    public async Task GetUserMessagesAsync_ValidInput_ReturnsOkObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedMessages = new List<Message> { new Message { Id = Guid.NewGuid() }, new Message { Id = Guid.NewGuid() } };
        _messageServiceMock.Setup(service => service.GetUserMessagesAsync(userId)).ReturnsAsync(expectedMessages);

        // Act
        var result = await _controller.GetUserMessagesAsync(userId);

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<Message>>>(result);
        var returnedMessages = Assert.IsAssignableFrom<IEnumerable<Message>>(okResult.Value);

        Assert.Equal(expectedMessages.Count, returnedMessages.Count());
        _messageServiceMock.Verify(service => service.GetUserMessagesAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserMessageAsync_ValidInput_ReturnsOkObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var messageId = Guid.NewGuid();
        var expectedMessage = new Message { Id = messageId };
        _messageServiceMock.Setup(service => service.GetUserMessageAsync(userId, messageId)).ReturnsAsync(expectedMessage);

        // Act
        var result = await _controller.GetUserMessageAsync(userId, messageId);

        // Assert
        var okResult = Assert.IsType<ActionResult<Message>>(result);
        var returnedMessage = Assert.IsType<Message>(okResult.Value);

        Assert.Equal(expectedMessage.Id, returnedMessage.Id);
        _messageServiceMock.Verify(service => service.GetUserMessageAsync(userId, messageId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserMessageAsync_ValidInput_ReturnsNoContentResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var messageId = Guid.NewGuid();
        _messageServiceMock.Setup(service => service.DeleteUserMessageAsync(userId, messageId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUserMessageAsync(userId, messageId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _messageServiceMock.Verify(service => service.DeleteUserMessageAsync(userId, messageId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserMessagesAsync_ValidInput_ReturnsNoContentResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _messageServiceMock.Setup(service => service.DeleteUserMessagesAsync(userId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteUserMessagesAsync(userId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _messageServiceMock.Verify(service => service.DeleteUserMessagesAsync(userId), Times.Once);
    }
}