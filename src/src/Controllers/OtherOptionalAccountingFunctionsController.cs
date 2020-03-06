using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class OtherOptionalAccountingFunctionsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public OtherOptionalAccountingFunctionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OtherOptionalAccountingFunctions(Guid org)
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