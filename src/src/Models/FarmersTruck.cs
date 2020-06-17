using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class FarmersTruck
    {
        [Key]
        public Guid ticketingId { get; set; }
        //[Required]
        [Display(Name = "Date and time")]
        public DateTime? DateInspected { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        [Required]
        [Display(Name = "Stall number")]
        public string StallNumber { get; set; }

        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Required]
        [Display(Name = "Farmers name")]
        public string FarmersName { get; set; }

        [Required]
        [Display(Name = "Organization")]
        public string Organization { get; set; }

        [Required]
        [Display(Name = "Commodity")]
        public string Commodity { get; set; }

        //[Required]
        [Display(Name = "Volume")]
        public int? Volume { get; set; }

        [Required]
        [Display(Name = "Barangay")]
        public string Barangay { get; set; }

        [Display(Name = "Municipality")]
        public string Municipality { get; set; }

        [Display(Name = "Province")]
        public string Province { get; set; }

        [Display(Name = "FacilitatorsName")]
        public string FacilitatorsName { get; set; }

        public string Inspector { get; set; }

        public string ParkingNumber { get; set; }

        public string AccreditationChecker { get; set; }

        public string Remarks { get; set; }

    }
}
