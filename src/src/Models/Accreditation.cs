using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Accreditation
    {
        public Accreditation()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid accreditationId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Accreditation Id")]
        public string plateNumber { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Plate Number")]

        public string farmerName { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Total Land Area")]
        public string totalLandArea { get; set; }

        [StringLength(300)]
        [Display(Name = "Crops")]
        public string crops { get; set; }
        [StringLength(300)]
        [Display(Name = "Area Planted")]
        public string areaPlanted { get; set; }

        [Display(Name = "Month Planted")]
        public string monthPlanted { get; set; }

        [Display(Name = "Month Harvested")]
        public string monthHarvested { get; set; }

        [Display(Name = "Municipality")]
        public string municipality { get; set; }

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
