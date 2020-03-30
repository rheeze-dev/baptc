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

        public string Counter { get; set; }

        public string Association { get; set; }

        public int ReferenceNumber { get; set; }

        public int IdNumber { get; set; }

        public DateTime DateOfApplication { get; set; }

        public string Name { get; set; }

        public string SpouseName { get; set; }

        public string Sitio { get; set; }

        public string Barangay { get; set; }

        public string Municipality { get; set; }

        public string Province { get; set; }

        public string ContactNumber { get; set; }

        public string BirthDate { get; set; }

        public string PlateNumber { get; set; }

        public string EstimatedTotalLandArea { get; set; }

        public string MajorCrops { get; set; }

        public string LandAreaPerCrop { get; set; }

        public int EstimatedProduce { get; set; }

        public string Planting { get; set; }

        public string Harvesting { get; set; }

    }
}
