using System.Collections.Generic;
using System;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public Guid UserUniqueIdentity { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime DateofBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created {get; set;}
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor {get; set;}
        public string Interests {get; set;}
        public ICollection<PhoneNumberDetails> PhoneNumber { get; set; }
        public string Email { get; set; }
        public float? Location { get; set; }
        public string PersonalityType {get; set;}
        public string City {get; set;}
        public string Country {get; set;} 
        public ICollection<Photo> Photos { get; set; }
  }
}