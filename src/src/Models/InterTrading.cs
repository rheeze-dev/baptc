using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class InterTrading
    {

        public int Id { get; set; }

        public int? Code { get; set; }
        //[Required]
        public DateTime Date { get; set; }
        //[Required]
        public string FarmerName { get; set; }

        public string FarmersOrganization { get; set; }

        public string Commodity { get; set; }

        public int Volume { get; set; }

        public string ProductionArea { get; set; }

        public string Inspector { get; set; }

    }
}
