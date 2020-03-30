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

        public string Barangay { get; set; }

        public string Municipality { get; set; }

        public string Province { get; set; }

        public string ContactNumber { get; set; }

        public string BirthDate { get; set; }

        public int Tin { get; set; }

        public string BusinessName { get; set; }

        public string BusinessAddress { get; set; }

        public string VehiclePlateNumber { get; set; }

        public string ProductDestination { get; set; }
    }
}