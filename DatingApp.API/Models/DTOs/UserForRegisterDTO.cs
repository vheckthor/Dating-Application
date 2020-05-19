using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(20,MinimumLength=6, ErrorMessage="You must specify password between 6 to 20 characters")]
        public string password { get; set; }
        
        [Required]
        public string Gender { get; set; }
        
        [Required]
        public string KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string Country { get; set; }
        [Required]
        public ICollection<PhoneNumberDetailsDTO> PhoneNumber { get; set; }
        public DateTime Created { get; set; }   
        public DateTime LastActive { get; set; }
       
        public UserForRegisterDTO()
        {
            Created= DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}