using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class TradersTruck
    {

        //public int Id { get; set; }
        [Key]
        public Guid ticketingId { get; set; }
        //[Required]
        [Display(Name = "Date and time")]
        public DateTime? DateInspected { get; set; }

        [Required]
        [Display(Name = "Traders name")]
        public string TraderName { get; set; }

        //[Required]
        [Display(Name = "Estimated volume (kg)")]
        public int? EstimatedVolume { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public string PlateNumber { get; set; }

        public string Inspector { get; set; }
    }
}
