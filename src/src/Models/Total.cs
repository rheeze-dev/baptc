using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Total
    {
        public int Id { get; set; }

        public Guid ticketingId { get; set; }

        public string origin { get; set; }

        public DateTime date { get; set; }

        public int amount { get; set; }

    }
}
