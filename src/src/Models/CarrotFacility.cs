using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class CarrotFacility
    {

        public int Id { get; set; }

        public int? Code { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string StallNumber { get; set; }

        [Required]
        public string Facilitator { get; set; }

        [Required]
        public string Commodity { get; set; }

        [Required]
        public int? Volume { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Inspector { get; set; }

    }
}
