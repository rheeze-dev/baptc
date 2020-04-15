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

        [Required]
        public string NameOfAssociation { get; set; }

        [Required]
        public string PackerOrPorter { get; set; }

        [Required]
        public int? IdNumber { get; set; }

        public DateTime DateOfApplication { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public string NameOfSpouse { get; set; }

        [Required]
        public string PresentAddress { get; set; }

        [Required]
        public string Barangay { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Province { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        public string ProvincialAddress { get; set; }

        [Required]
        public string Requirements { get; set; }

    }
}