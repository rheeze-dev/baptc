using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "Commodity")]
        public string commodity { get; set; }

        [Required]
        [Display(Name = "Class Variety")]
        public string classVariety { get; set; }

        [Required]
        [Display(Name = "Price Range")]
        public double? priceRange { get; set; }

        [Display(Name = "Commodity Remarks")]
        public string commodityRemarks { get; set; }

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
