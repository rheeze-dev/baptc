using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Traders
    {
        public Traders()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid tradersId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Traders Id")]
        public string traderName { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Name of Trader")]

        public string address { get; set; }

        [Display(Name = "Contact Number")]
        public string contactNumber { get; set; }

        [Display(Name = "Stall Id")]
        public int stallId { get; set; }

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
