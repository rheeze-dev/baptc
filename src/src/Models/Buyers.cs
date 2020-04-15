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

        [Required]
        public string NameOfSpouse { get; set; }

        [Required]
        public string PresentAddress { get; set; }

        [Required]
        public string Barangay { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public int? Tin { get; set; }

        [Required]
        public string BusinessName { get; set; }

        [Required]
        public string BusinessAddress { get; set; }

        [Required]
        public string VehiclePlateNumber { get; set; }

        [Required]
        public string ProductDestination { get; set; }
    }
}