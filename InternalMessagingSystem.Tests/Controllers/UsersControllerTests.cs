using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalMessagingSystem.Controllers;
using InternalMessagingSystem.Models;
using InternalMessagingSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InternalMessagingSystem.Tests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task AddUserAsync_WithValidName_ReturnsCreatedUser()
        {
            // Arrange
            var userName = "testuser";
            var userServiceMock = new Mock<IUserService>();
            var usersController = new UsersController(userServiceMock.Object);

            var user = new User { Id = Guid.NewGuid(), Name = userName };
            userServiceMock.Setup(service => service.CreateUserAsync(userName)).ReturnsAsync(user);

            // Act
            var result = await usersController.AddUserAsync(userName);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedUser = Assert.IsAssignableFrom<User>(createdAtActionResult.Value);

            Assert.Equal(user.Id, returnedUser.Id);
            Assert.Equal(user.Name, returnedUser.Name);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsListOfUsers()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var usersController = new UsersController(userServiceMock.Object);

            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), Name = "user1" },
                new User { Id = Guid.NewGuid(), Name = "user2" }
            };
            userServiceMock.Setup(service => service.GetUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await usersController.GetUsersAsync();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);

            Assert.Equal(users.Count, returnedUsers.Count());
        }

        [Fact]
        public async Task GetUserAsync_WithExistingUserId_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userServiceMock = new Mock<IUserService>();
            var usersController = new UsersController(userServiceMock.Object);

            var user = new User { Id = userId, Name = "testuser" };
            userServiceMock.Setup(service => service.GetUserAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await usersController.GetUserAsync(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<User>>(result);
            var returnedUser = Assert.IsType<User>(actionResult.Value);

            Assert.Equal(user.Id, returnedUser.Id);
            Assert.Equal(user.Name, returnedUser.Name);
        }

        [Fact]
        public async Task DeleteUserAsync_WithExistingUserId_ReturnsNoContent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userServiceMock = new Mock<IUserService>();
            var usersController = new UsersController(userServiceMock.Object);

            userServiceMock.Setup(service => service.DeleteUserAsync(userId)).Returns(Task.CompletedTask);

            // Act
            var result = await usersController.DeleteUserAsync(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
