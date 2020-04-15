using System;
using System.ComponentModel.DataAnnotations;

namespace src.Models
{
    public class TicketingPrice
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }

        [Required]
        public int? TradersTruckWithTransaction { get; set; }

        [Required]
        public int? TradersTruckWithoutTransaction { get; set; }

        [Required]
        public int? FarmersTruckSingleTire { get; set; }

        [Required]
        public int? FarmersTruckDoubleTire { get; set; }

        [Required]
        public int? ShortTripPickUp { get; set; }

        [Required]
        public int? ShortTripDelivery { get; set; }

        [Required]
        public int? PayParkingDaytime { get; set; }

        [Required]
        public int? PayParkingOvernight { get; set; }

        [Required]
        public int? StallLeasePrice { get; set; }

        public string Editor { get; set; }
    }
}
