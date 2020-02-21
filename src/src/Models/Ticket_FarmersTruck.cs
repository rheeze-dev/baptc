using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Ticket_FarmersTruck : BaseEntity
    {
        public Ticket_FarmersTruck()
        {
            this.ticketStatus = Enum.TicketStatus.Unassigned;
            this.ticketType = Enum.TicketType.Problem;
            this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid ticketId { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Stall Number")]
        public string ticketName { get; set; }
        //[Required]
        [StringLength(200)]
        [Display(Name = "Plate Number")]
        public string description { get; set; }

        [Display(Name = "Customer ID")]
        public Guid customerId { get; set; }

        [Display(Name = "Commodity")]
        public Guid contactId { get; set; }

        [StringLength(100)]
        [Display(Name = "Farmers Name")]
        public string email { get; set; }
        [StringLength(20)]
        [Display(Name = "Organization")]
        public string phone { get; set; }

        [Display(Name = "Volume (kgs)")]
        public Enum.TicketStatus ticketStatus { get; set; }

        [Display(Name = "Barangay")]
        public Guid supportAgentId { get; set; }

        [Display(Name = "Province")]
        public Guid supportEngineerId { get; set; }

        [Display(Name = "Date")]
        public Guid productId { get; set; }

        [Display(Name = "Facilitators Name")]
        public Enum.TicketType ticketType { get; set; }
        [Display(Name = "Ticket Priority")]
        public Enum.TicketPriority ticketPriority { get; set; }
        [Display(Name = "Destination")]
        public Enum.TicketChannel ticketChannel { get; set; }

        public Guid organizationId { get; set; }
        public Organization organization { get; set; }

    }
}
