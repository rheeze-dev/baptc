using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Clerk
    {
        public Clerk()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid clerkId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Clerk Id")]
        public DateTime clerkDate { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Clerk Date")]

        public string orNumber { get; set; }

        [Display(Name = "Payor")]
        public string payor { get; set; }

        [Display(Name = "Classification")]
        public string classification { get; set; }

        [StringLength(300)]
        [Display(Name = "Total Amount")]
        public double totalAmount { get; set; }
        [StringLength(300)]
        [Display(Name = "Month Paid")]
        public string monthPaid { get; set; }

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
