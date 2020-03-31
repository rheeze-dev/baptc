using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class CurrentTicket
    {
       [Key]
        public Guid ticketingId { get; set; }

        public string plateNumber { get; set; }

        public string typeOfTransaction { get; set; }

    }
}
