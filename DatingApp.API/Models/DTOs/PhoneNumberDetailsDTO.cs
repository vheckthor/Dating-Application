using System;

namespace DatingApp.API.Models.DTOs
{
    public class PhoneNumberDetailsDTO
    {
        public  string CountryCode { get; set; }
        public string DialCode  { get; set; }
        public string  Identifier { get; set; }
        public string InternationalNumber { get; set; }
        public string nationalNumber  { get; set; } 
    }
}