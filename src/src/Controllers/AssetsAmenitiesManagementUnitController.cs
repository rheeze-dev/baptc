using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class AssetsAmenitiesManagementUnitController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AssetsAmenitiesManagementUnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AssetsAmenitiesManagementUnit(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

    }
}