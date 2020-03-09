using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class SecurityRepairCheck
    {

        public int Id { get; set; }
        //[Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        //[Required]
        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Repair details")]
        public string RepairDetails { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

    }
}
