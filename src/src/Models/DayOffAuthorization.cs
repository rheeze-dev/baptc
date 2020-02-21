using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class DayOffAuthorization
    {
        public DayOffAuthorization()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid dayOffAuthorizationId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Day Of Authorization Id")]
        public string tasks { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Tasks")]

        public string expectedOutput { get; set; }

        [Display(Name = "Supervisor")]
        public string supervisor { get; set; }

        [Display(Name = "Absence Id")]
        public string absenceId { get; set; }

        [StringLength(300)]
        [Display(Name = "Approve Status")]
        public string approveStatus { get; set; }
        [StringLength(300)]
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
