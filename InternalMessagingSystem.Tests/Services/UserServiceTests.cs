using InternalMessagingSystem.Services;
using InternalMessagingSystem.Exceptions;

// This class contains unit tests for the UserService class.
namespace InternalMessagingSystem.Tests
{
    public class UserServiceTests
    {
        private UserService _userService;

        // Constructor to setup initial test environment
        public UserServiceTests()
        {
            _userService = new UserService();
        }

        // Test for CreateUserAsync method to ensure it returns the created user.
        [Fact]
        public async Task CreateUserAsync_ReturnsCreatedUser()
        {
            // ARRANGE: Define the test user name
            var name = "Test User";

            // ACT: Call CreateUserAsync method
            var user = await _userService.CreateUserAsync(name);

            // ASSERT: Check if the returned user is not null and if its name is as expected
            Assert.NotNull(user);
            Assert.Equal(name, user.Name);
        }

        // Test for GetUserAsync method to check it returns the correct user when user exists.
        [Fact]
        public async Task GetUserAsync_WhenUserExists_ReturnsUser()
        {
            // ARRANGE: Create a test user
            var name = "Test User";
            var createdUser = await _userService.CreateUserAsync(name);

            // ACT: Call GetUserAsync method
            var retrievedUser = await _userService.GetUserAsync(createdUser.Id);

            // ASSERT: Check if the retrieved user matches the created user
            Assert.NotNull(retrievedUser);
            Assert.Equal(createdUser.Id, retrievedUser.Id);
            Assert.Equal(createdUser.Name, retrievedUser.Name);
        }

        // Test for GetUserAsync method to check it throws exception when user does not exist.
        [Fact]
        public async Task GetUserAsync_WhenUserDoesNotExist_ThrowsException()
        {
            // ARRANGE: Define a non-existent user ID
            var userId = Guid.NewGuid();

            // ACT and ASSERT: Assert if the method throws a UserNotFoundException
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserAsync(userId));
        }

        // Test for DeleteUserAsync method to ensure it deletes user when user exists.
        [Fact]
        public async Task DeleteUserAsync_WhenUserExists_DeletesUser()
        {
            // ARRANGE: Create a test user
            var name = "Test User";
            var user = await _userService.CreateUserAsync(name);

            // ACT: Call DeleteUserAsync method
            await _userService.DeleteUserAsync(user.Id);

            // ASSERT: Check if the user was successfully deleted by expecting a UserNotFoundException
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserAsync(user.Id));
        }

        // Test for DeleteUserAsync method to check it throws exception when user does not exist.
        [Fact]
        public async Task DeleteUserAsync_WhenUserDoesNotExist_ThrowsException()
        {
            // ARRANGE: Define a non-existent user ID
            var userId = Guid.NewGuid();

            // ACT and ASSERT: Assert if the method throws a UserNotFoundException
            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.DeleteUserAsync(userId));
        }

        // Test for GetUsersAsync method to check it returns all users.
        [Fact]
        public async Task GetUsersAsync_ReturnsAllUsers()
        {
            // ARRANGE: Create multiple test users
            var names = new List<string> { "Test User 1", "Test User 2", "Test User 3" };
            foreach (var name in names)
            {
                await _userService.CreateUserAsync(name);
            }

            // ACT: Call GetUsersAsync method
            var users = await _userService.GetUsersAsync();

            // ASSERT: Check if the number of returned users is as expected
            Assert.Equal(names.Count, users.Count);
        }
    }
}
