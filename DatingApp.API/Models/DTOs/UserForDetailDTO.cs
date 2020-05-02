using System;
using System.Collections.Generic;

namespace DatingApp.API.Models.DTOs
{
    public class UserForDetailDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created {get; set;}
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor {get; set;}
        public string Interests {get; set;}
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public float? Location { get; set; }
        public string PersonalityType {get; set;}
        public string City {get; set;}
        public string Country {get; set;} 
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForDetailsDTO> Photos { get; set; }
    }
}