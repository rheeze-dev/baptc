using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Finance
    {
        public Finance()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid financeId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Finance Id")]
        public string financeName { get; set; }
        //[Required]
        //[StringLength(300)]
        //[Display(Name = "Name of Finance")]

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
