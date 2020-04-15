using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Repair
    {

        public int Id { get; set; }
        //[Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string DriverName { get; set; }

        [Required]
        [Display(Name = "Repair details")]
        public string RequesterName { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string RepairDetails { get; set; }

        public int? RequestNumber { get; set; }

    }
}
