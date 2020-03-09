using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class SecurityController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SecurityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult SecurityRepairCheck(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult SecurityInspectionReport(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditSecurityRepairCheck(Guid org, int id)
        {
            if (id == 0)
            {
                SecurityRepairCheck securityRepairCheck = new Models.SecurityRepairCheck();
                //ticketing.ticketingId = org;
                return View(securityRepairCheck);
            }
            else
            {
                return View(_context.SecurityRepairCheck.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }
        }

        public IActionResult AddEditSecurityInspectionReport(Guid org, int id)
        {
            if (id == 0)
            {
                SecurityInspectionReport securityInspectionReport = new Models.SecurityInspectionReport();
                //ticketing.ticketingId = org;
                return View(securityInspectionReport);
            }
            else
            {
                return View(_context.SecurityInspectionReport.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }
        }

    }
}