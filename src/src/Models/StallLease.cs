﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class StallLease
    {
       [Key]
        public Guid ticketingId { get; set; }
        //public DateTime BirthDate { get; set; }
        public string DriverName { get; set; }
        //public string LastName { get; set; }
        public string PlateNumber1 { get; set; }
        public string PlateNumber2 { get; set; }
        //public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public DateTime? NewStartDate { get; set; }
        public string ContactNumber { get; set; }
        public string StallNumber { get; set; }
        //public int IdNumber { get; set; }
        public string Remarks { get; set; }
        public int Amount { get; set; }
        public int? ControlNumber { get; set; }

    }
}
