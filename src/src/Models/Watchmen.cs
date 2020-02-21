using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Watchmen
    {
        public Watchmen()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid watchmenId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Watchmen Id")]
        public string watchmenName { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Watchmen Name")]

        public string repairCheck { get; set; }

        [Display(Name = "Other Reports")]
        public string otherReports { get; set; }

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
