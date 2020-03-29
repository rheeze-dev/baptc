using System;

namespace src.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RoleId { get; set; }
        //public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
        public string Modules { get; set; }
    }
}
