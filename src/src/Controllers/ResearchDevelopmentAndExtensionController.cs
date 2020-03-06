using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class ResearchDevelopmentAndExtensionController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ResearchDevelopmentAndExtensionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ResearchDevelopmentAndExtension(Guid org)
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