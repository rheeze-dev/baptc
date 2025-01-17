﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            this.thumbUrl = "/images/no-image-available.png";
            this.customerType = Enum.CustomerType.Internal;
        }
        public Guid customerId { get; set; }
        [Display(Name = "Trader Truck")]
        [StringLength(100)]
        [Required]
        public string customerName { get; set; }
        [StringLength(200)]
        [Display(Name = "Farmer Truck")]
        public string description { get; set; }
        [StringLength(255)]
        [Display(Name = "Thumb Url")]
        public string thumbUrl { get; set; }
        [Display(Name = "Location")]
        public Enum.CustomerType customerType { get; set; }

        //address
        [Display(Name = "Short Trip")]
        [StringLength(100)]
        public string address { get; set; }
        [Display(Name = "Pay Parking")]
        [StringLength(20)]
        public string phone { get; set; }
        [Display(Name = "Plate Number")]
        [StringLength(100)]
        public string email { get; set; }
        [Display(Name = "Time In")]
        [StringLength(100)]
        public string website { get; set; }
        [Display(Name = "Time Out")]
        [StringLength(100)]
        public string linkedin { get; set; }

        public Guid organizationId { get; set; }
        public Organization organization { get; set; }

        //contacts
        public ICollection<Contact> contacts { get; set; }
    }
}
