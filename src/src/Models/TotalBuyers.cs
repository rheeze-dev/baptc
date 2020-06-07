using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class TotalBuyers
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Total { get; set; }
    }
}