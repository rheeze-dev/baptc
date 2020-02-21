using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Compensatory
    {
        public Compensatory()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid compensatoryId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Compensatory Id")]
        public DateTime requestDate { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Request Date")]

        public string supervisor { get; set; }

        [Display(Name = "Available Days")]
        public string daysAvailable { get; set; }

        [Display(Name = "Approval Status")]
        public string approvalStatus { get; set; }

        [StringLength(300)]
        [Display(Name = "Application Date")]
        public DateTime applicationDate { get; set; }

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
