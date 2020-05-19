using System;

namespace DatingApp.API.Models
{
    public class PhoneNumberDetails
    {
        public int Id { get; set; }
        public Guid UserIdentifier { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public  string CountryCode { get; set; }
        public string DialCode  { get; set; }
        public string  Identifier { get; set; }
        public string InternationalNumber { get; set; }
        public string nationalNumber  { get; set; } 
    }
}