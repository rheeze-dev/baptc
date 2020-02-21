using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Ticketing
    {
        public Ticketing()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid ticketingId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Ticketing Id")]
        public DateTime timeIn { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Time In")]

        public DateTime timeOut { get; set; }

        [Display(Name = "Plate Number")]
        public string plateNumber { get; set; }

        [Display(Name = "Type of Transaction")]
        public string typeOfTransaction { get; set; }

        [StringLength(300)]
        [Display(Name = "Date of Gate Pass")]
        public string gatePassDate { get; set; }

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
