using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Stalls
    {
        public Stalls()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid stallsId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Stalls Id")]
        public string stallOwner { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Stall Owner")]

        public string payment { get; set; }

        [Display(Name = "Transfer Request")]
        public string transferRequest { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

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
