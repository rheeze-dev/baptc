using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class HrForm
    {
        public HrForm()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid HrFormId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "HR Form Id")]
        public string compensatory { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Compensatory")]

        public string absence { get; set; }

        [Display(Name = "Day Off Report")]
        public string dayOffReport { get; set; }

        [Display(Name = "Requested Date")]
        public DateTime requestDate { get; set; }

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
