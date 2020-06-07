using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class DailyBuyers
    {

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string PlateNumber { get; set; }

        public int Count { get; set; }

    }
}