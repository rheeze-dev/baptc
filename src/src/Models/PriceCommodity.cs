using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class PriceCommodity
    {
        public PriceCommodity()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid priceCommodityId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Price Commodity Id")]
        public DateTime time { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Time")]

        public DateTime commodityDate { get; set; }

        [Display(Name = "Commodity")]
        public string commodity { get; set; }

        [Display(Name = "Class Variety")]
        public string classVariety { get; set; }

        [StringLength(300)]
        [Display(Name = "Price Range")]
        public double priceRange { get; set; }
        [StringLength(300)]
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
