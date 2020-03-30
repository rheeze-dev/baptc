using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class PackersAndPorters
    {

        public int Id { get; set; }

        public string NameOfAssociation { get; set; }

        public string PackerOrPorter { get; set; }

        public int IdNumber { get; set; }

        public DateTime DateOfApplication { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public string NameOfSpouse { get; set; }

        public string PresentAddress { get; set; }

        public string Barangay { get; set; }

        public string Municipality { get; set; }

        public string Province { get; set; }

        public string ContactNumber { get; set; }

        public string BirthDate { get; set; }

        public string ProvincialAddress { get; set; }

        public string Requirements { get; set; }

    }
}