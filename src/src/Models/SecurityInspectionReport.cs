using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class SecurityInspectionReport
    {

        public int Id { get; set; }
        //[Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        //[Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

    }
}
