using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class GatePass
    {
       
        public int Id { get; set; }
        //[Required]
        //[Display(Name = "Time In")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime Date { get; set; }
        //[Required]

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        //[Display(Name = "Time Out")]
        public string Name { get; set; }

        //[Display(Name = "Plate Number")]
        public string PlateNumber1 { get; set; }

        //[Display(Name = "Type Of Transaction")]
        public string PlateNumber2 { get; set; }

        //[Display(Name = "Gate Pass Date")]
        //public string gatePassDate { get; set; }

    }
}
