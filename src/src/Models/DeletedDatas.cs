using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class DeletedDatas
    {

        public int Id { get; set; }

        public DateTime DateDeleted { get; set; }

        public string PlateNumber { get; set; }

        public string Origin { get; set; }

        public string Name { get; set; }

        public string DeletedBy { get; set; }

        public string Remarks { get; set; }

        public int? Amount { get; set; }
    }
}