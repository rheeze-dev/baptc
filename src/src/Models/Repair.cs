using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Repair
    {
        public Repair()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid repairId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Repair Id")]
        public string sideMarkings { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Side Markings")]

        public string ownerName { get; set; }

        [Display(Name = "Driver Name")]
        public string driverName { get; set; }

        [Display(Name = "Requested Name")]
        public string requestedName { get; set; }

        [StringLength(300)]
        [Display(Name = "Repair Details")]
        public string repairDetails { get; set; }
        [StringLength(300)]
        [Display(Name = "Destination")]
        public string destination { get; set; }

        [Display(Name = "Contact Number")]
        public string contactNumber { get; set; }

        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Repair Time")]
        public DateTime repairTime { get; set; }

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
