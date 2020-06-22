using System;

namespace DatingApp.API.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }
        public int SenderId { get; set; }
        public Guid SenderUniqueId { get; set; }
        public int RecipientId { get; set; }
        public Guid RecipientUniqueId { get; set; }
        public User Recipient { get; set; }
        public User Sender { get; set; }    
        public string Content { get; set; }
        public bool IsRead { get; set; }    
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
        public bool SenderDeleted { get; set; }
        public bool DeletedForBoth { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}