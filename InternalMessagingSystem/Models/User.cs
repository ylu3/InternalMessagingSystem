namespace InternalMessagingSystem.Models
{
    /// <summary>
    /// Represents a user in the internal messaging system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
