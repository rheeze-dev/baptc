using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class InterTraders
    {

        public int Id { get; set; }

        public string Counter { get; set; }

        public string NameOfAssociation { get; set; }

        public int ReferenceNumber { get; set; }

        public int IdNumber { get; set; }

        public DateTime DateOfApplication { get; set; }

        public string Name { get; set; }

        public string NameOfSpouse { get; set; }

        public string PresentAddress { get; set; }

        public string Barangay { get; set; }

        public string Municipality { get; set; }

        public string Province { get; set; }

        public string ContactNumber { get; set; }

        public int Tin { get; set; }

        public string BusinessPermit { get; set; }

        public string Destination { get; set; }

    }
}
