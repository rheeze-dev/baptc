using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class PriceCommodity
    {
        public Guid priceCommodityId { get; set; }
        //[Required]
        [Display(Name = "Time")]
        public DateTime time { get; set; }
        //[Required]

        [Display(Name = "Commodity Date")]
        public DateTime commodityDate { get; set; }

        public string enteredBy { get; set; }

        [Required]
        [Display(Name = "Commodity")]
        public string commodity { get; set; }

        [Required]
        [Display(Name = "Class Variety")]
        public string classVariety { get; set; }

        [Required]
        public double? priceLow { get; set; }

        [Required]
        public double? priceHigh { get; set; }

        public double? averageLow { get; set; }

        public double? averageHigh { get; set; }

        [Required]
        public int totalDays { get; set; }

        [Display(Name = "Commodity Remarks")]
        public string commodityRemarks { get; set; }

        [NotMapped]
        public IList<SelectListItem> commodityList { get; set; }

        //[Display(Name = "Ticket Type")]
        //public Enum.TicketType ticketType { get; set; }
        //[Display(Name = "Ticket Priority")]
        //public Enum.TicketPriority ticketPriority { get; set; }
        //[Display(Name = "Destination")]
        //public Enum.TicketChannel ticketChannel { get; set; }

        //public Guid organizationId { get; set; }
        //public Organization organization { get; set; }

    }
}
