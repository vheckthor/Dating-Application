using System;

namespace DatingApp.API.Models.DTOs
{
    public class PhotoForDetailsDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public Guid PhotoUniqueIdentifier { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
    }
}