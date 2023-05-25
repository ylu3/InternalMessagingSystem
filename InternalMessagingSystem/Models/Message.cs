using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InternalMessagingSystem.Models
{
    /// <summary>
    /// Represents a message in the internal messaging system.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The unique identifier for the message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the sender.
        /// </summary>
        public Guid SenderId { get; set; }

        /// <summary>
        /// The ID of the receiver.
        /// </summary>
        public Guid ReceiverId { get; set; }

        /// <summary>
        /// The timestamp when the message was sent.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// The content of the message.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the message has been deleted by the sender.
        /// </summary>
        [JsonIgnore]
        public bool IsDeletedBySender { get; set; }

        /// <summary>
        /// Indicates whether the message has been deleted by the receiver.
        /// </summary>
        [JsonIgnore]
        public bool IsDeletedByReceiver { get; set; }
    }

    /// <summary>
    /// Data transfer object for creating a message.
    /// </summary>
    public class CreateMessageDTO
    {
        /// <summary>
        /// The ID of the receiver.
        /// </summary>
        [Required]
        public Guid? ReceiverId { get; set; }


        /// <summary>
        /// The content of the message.
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
