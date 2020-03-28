using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class ShortTrip
    {
        [Key]
        public Guid ticketingId { get; set; }
        //[Required]
        [Display(Name = "Time in")]
        public DateTime TimeIn { get; set; }
        //[Required]
        [Display(Name = "Time out")]
        public DateTime? TimeOut { get; set; }

        public DateTime? DateInspected { get; set; }

        [Display(Name = "Estimated volume (kg)")]
        public int? EstimatedVolume { get; set; }

        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Required]
        [Display(Name = "Commodity")]
        public string Commodity { get; set; }

        public string Inspector { get; set; }
    }
}
