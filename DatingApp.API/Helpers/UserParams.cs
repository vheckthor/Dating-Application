using System;

namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 15;
        public int PageNUmber { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value > MaxPageSize)?MaxPageSize : value;}
        }
        public Guid UserId { get; set; }
        public string Gender { get; set; }
        
    }
}