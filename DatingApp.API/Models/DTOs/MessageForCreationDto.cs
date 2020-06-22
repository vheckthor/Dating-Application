using System;

namespace DatingApp.API.Models.DTOs
{
    public class MessageForCreationDto
    {
        public Guid UniqueId { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public Guid SenderUniqueId { get; set; }
        public Guid RecipientUniqueId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
            UniqueId = Guid.NewGuid();
        }
    }
}