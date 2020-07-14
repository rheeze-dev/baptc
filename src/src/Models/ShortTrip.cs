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

        public Guid ticketingId { get; set; }
        //[Required]
        [Display(Name = "Time in")]
        public DateTime TimeIn { get; set; }
        //[Required]
        [Display(Name = "Time out")]
        public DateTime? TimeOut { get; set; }

        public DateTime? DateInspectedIn { get; set; }

        public DateTime? DateInspectedOut { get; set; }

        //[Required]
        [Display(Name = "Estimated volume (kg)")]
        public int? EstimatedVolumeIn { get; set; }

        [Display(Name = "Estimated volume (kg)")]
        public int? EstimatedVolumeOut { get; set; }

        [Display(Name = "Plate number")]
        public string PlateNumber { get; set; }

        [Required]
        [Display(Name = "Commodity")]
        public string CommodityIn { get; set; }

        [Required]
        [Display(Name = "Commodity")]
        public string CommodityOut { get; set; }

        public string ParkingNumber { get; set; }

        public string TypeOfEntry { get; set; }

        public string InspectorIn { get; set; }

        public string InspectorOut { get; set; }

        public string RemarksIn { get; set; }

        public string RemarksOut { get; set; }

    }
}
