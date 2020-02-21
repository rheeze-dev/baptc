using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Inspector
    {
        public Inspector()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid inspectorId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Inspector Id")]
        public string inspectorName { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Name of Inspector")]

        public DateTime dateChecked { get; set; }

        [Display(Name = "Type of Transaction")]
        public string typeOfTransaction { get; set; }

        [Display(Name = "Control Id")]
        public string controlId { get; set; }

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
