using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Buyers
    {

        public int Id { get; set; }

        public string NameOfSpouse { get; set; }

        public string PresentAddress { get; set; }

        [Required]
        public string Barangay { get; set; }

        public string Municipality { get; set; }

        public string Province { get; set; }

        public string ContactNumber { get; set; }

        public string BirthDate { get; set; }

        public string Tin { get; set; }

        [Required]
        public string BusinessName { get; set; }

        public string BusinessAddress { get; set; }

        [Required]
        public string VehiclePlateNumber { get; set; }

        public string ProductDestination { get; set; }

        public DateTime DateOfApplication { get; set; }

        public string Remarks { get; set; }

        public string EnteredBy { get; set; }

    }
}