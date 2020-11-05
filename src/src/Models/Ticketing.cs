using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Ticketing
    {
       
        public Guid ticketingId { get; set; }
        //[Required]
        [Display(Name = "Time In")]

        public DateTime? timeIn { get; set; }
        //[Required]

        [Display(Name = "Time Out")]
        public DateTime? timeOut { get; set; }

        [Required]
        [Display(Name = "Plate Number")]
        public string plateNumber { get; set; }

        [Display(Name = "Type Of Transaction")]
        public string typeOfTransaction { get; set; }

        [Display(Name = "Type of car")]
        public string typeOfCar { get; set; }

        public string driverName { get; set; }

        public DateTime? endDate { get; set; }

        public int? amount { get; set; }

        public int? count { get; set; }

        public string remarks { get; set; }

        public int? controlNumber { get; set; }

        public string issuingClerk { get; set; }

        public string receivingClerk { get; set; }

        public string parkingNumber { get; set; }

        public string accreditation { get; set; }

        public string Transaction { get; set; }

        [NotMapped]
        public IList<SelectListItem> parkingList { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string ContactNumber { get; set; }

        public double Temperature { get; set; }

        public int Days { get; set; }

        public int Hours { get; set; }

        public string TypeOfPayment { get; set; }

        public int TotalCredit { get; set; }

        public int DebitAmount { get; set; }

        public string DebitReceiver { get; set; }

        public string TimeSpan { get; set; }

        public string PullOut { get; set; }

        public string TypeOfEntry { get; set; }

    }

}
