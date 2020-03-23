using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Ticketing
    {
       
        public Guid ticketingId { get; set; }
        //[Required]
        [Display(Name = "Time In")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime? timeIn { get; set; }
        //[Required]

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [Display(Name = "Time Out")]
        public DateTime? timeOut { get; set; }

        [Display(Name = "Plate Number")]
        public string plateNumber { get; set; }

        [Display(Name = "Type Of Transaction")]
        public string typeOfTransaction { get; set; }

        [Display(Name = "Type of car")]
        public string typeOfCar { get; set; }

        public string driverName { get; set; }

        public DateTime? endDate { get; set; }

        public int? amount { get; set; }

        public string remarks { get; set; }

    }
}
