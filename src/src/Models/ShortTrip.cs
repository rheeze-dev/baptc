using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class ShortTrip
    {

        public int Id { get; set; }
        //[Required]
        [Display(Name = "Time in")]
        public DateTime TimeIn { get; set; }
        //[Required]
        [Display(Name = "Time out")]
        public DateTime TimeOut { get; set; }
        //[Required]
        [Display(Name = "Estimated volume (kg)")]
        public int EstimatedVolume { get; set; }

        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Display(Name = "Commodity")]
        public string Commodity { get; set; }
    }
}
