using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class FarmersTruck
    {

        public int Id { get; set; }
        //[Required]
        [Display(Name = "Date and time")]
        public DateTime Date { get; set; }
        //[Required]
        [Display(Name = "Stall number")]
        public string StallNumber { get; set; }

        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Display(Name = "Farmers name")]
        public string FarmersName { get; set; }

        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Display(Name = "Commodity")]
        public string Commodity { get; set; }

        [Display(Name = "Volume")]
        public int Volume { get; set; }

        [Display(Name = "Barangay")]
        public string Barangay { get; set; }

        [Display(Name = "Province")]
        public string Province { get; set; }

        [Display(Name = "FacilitatorsName")]
        public string FacilitatorsName { get; set; }

    }
}
