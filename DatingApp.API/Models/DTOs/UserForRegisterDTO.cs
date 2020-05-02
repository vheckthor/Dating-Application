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
    }
}