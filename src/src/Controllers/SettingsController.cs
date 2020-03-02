using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Roles(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditRoles(Guid org, int id)
        {
            if (id == 0)
            {
                Roles roles = new Roles();
                //ticketing.ticketingId = org;
                return View(roles);
            }
            else
            {
                return View(_context.Role.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult UserRoles(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult ConfigRoles(Guid org, int id)
        {
            if (id == 0)
            {
                Roles roles = new Roles();
                //ticketing.ticketingId = org;
                return View(roles);
            }
            else
            {
                return View(_context.Role.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }
    }
}