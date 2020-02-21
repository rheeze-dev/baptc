using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class AbsenceRequest
    {
        public AbsenceRequest()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid absenceRequestId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Absence Request Id")]
        public string absenceType { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Type of Absence")]

        public DateTime inclusiveDates { get; set; }

        [Display(Name = "Total Number Of Days")]
        public int totalNumberOfDays { get; set; }

        [Display(Name = "Reasons")]
        public string reasons { get; set; }

        [StringLength(300)]
        [Display(Name = "Date of Filling")]
        public DateTime fillingDate { get; set; }
        [StringLength(300)]
        [Display(Name = "Approval Status")]
        public string approvalStatus { get; set; }

        [Display(Name = "Supervisor")]
        public string supervisor { get; set; }

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
