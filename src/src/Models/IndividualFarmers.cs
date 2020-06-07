using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class IndividualFarmers
    {

        public int Id { get; set; }

        [Required]
        public string Counter { get; set; }

        [Required]
        public string Association { get; set; }

        [Required]
        public int? ReferenceNumber { get; set; }

        [Required]
        public int? IdNumber { get; set; }

        [Required]
        public DateTime DateOfApplication { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SpouseName { get; set; }

        public string Sitio { get; set; }

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
        public string PlateNumber { get; set; }

        [Required]
        public string EstimatedTotalLandArea { get; set; }

        [Required]
        public string MajorCrops { get; set; }

        [Required]
        public string LandAreaPerCrop { get; set; }

        [Required]
        public int? EstimatedProduce { get; set; }

        [Required]
        public string Planting { get; set; }

        [Required]
        public string Harvesting { get; set; }

    }
}
