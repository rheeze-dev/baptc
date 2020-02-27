using System;

namespace src.Models
{
    public class Roles
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public DateTime DateAdded { get; set; }
        public string Remarks { get; set; }
        public string Module { get; set; }
    }
}
