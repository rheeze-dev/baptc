using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class PriceCommodityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PriceCommodityController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult Price(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult Graph(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditPrice(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                PriceCommodity priceCommodity = new PriceCommodity();
                //priceCommodity.organizationId = org;
                return View(priceCommodity);
            }
            else
            {
                return View(_context.PriceCommodity.Where(x => x.priceCommodityId.Equals(id)).FirstOrDefault());
            }

        }
    }
}