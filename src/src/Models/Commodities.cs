using System;

namespace src.Models
{
    public class Commodities
    {
        public int Id { get; set; }
        public string Commodity { get; set; }
        public string ClassVariety { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public string Modifier { get; set; }
        public DateTime Date { get; set; }

    }
}
