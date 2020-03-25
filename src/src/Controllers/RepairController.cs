using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class RepairController : Controller
    {

        private readonly ApplicationDbContext _context;

        public RepairController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult VehicleRepair(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditVehicleRepair(Guid org, int id)
        {
            if (id == 0)
            {
                Repair repair = new Repair();
                //ticketing.ticketingId = org;
                return View(repair);
            }
            else
            {
                return View(_context.Repair.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }
        }

    }
}