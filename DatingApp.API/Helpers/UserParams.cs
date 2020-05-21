using System;

namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 6;
        public int PageNUmber { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = (value >= MaxPageSize)?MaxPageSize : value;}
        }
        public Guid UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; }
        public bool Likees { get; set; } = false;
        public bool Likers { get; set; } = false;
        
    }
}