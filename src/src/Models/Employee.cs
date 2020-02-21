using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Employee
    {
        public Employee()
        {
            //this.ticketStatus = Enum.TicketStatus.Unassigned;
            //this.ticketType = Enum.TicketType.Problem;
            //this.ticketPriority = Enum.TicketPriority.Low;
        }

        public Guid employeeId { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "Employee Id")]
        public string employeeName { get; set; }
        //[Required]
        [StringLength(300)]
        [Display(Name = "Name of Employee")]

        public string userPassword { get; set; }

        [Display(Name = "Date of Employment")]
        public DateTime employmentDate { get; set; }

        [Display(Name = "Position")]
        public string position { get; set; }

        [StringLength(300)]
        [Display(Name = "Designation Office")]
        public string designationOffice { get; set; }
        [StringLength(300)]
        [Display(Name = "Total Attendance")]
        public string totalAttendance { get; set; }

        //[Display(Name = "Ticket Type")]
        //public Enum.TicketType ticketType { get; set; }
        //[Display(Name = "Ticket Priority")]
        //public Enum.TicketPriority ticketPriority { get; set; }
        //[Display(Name = "Destination")]
        //public Enum.TicketChannel ticketChannel { get; set; }

        //public Guid organizationId { get; set; }
        //public Organization organization { get; set; }

    }
}
