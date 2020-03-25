using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class TradingAndIntertradingController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TradingAndIntertradingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult TradingAndIntertrading(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult CarrotFacility(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditTradingAndIntertrading(Guid org, int id)
        {
            if (id == 0)
            {
                InterTrading interTrading = new InterTrading();
                //ticketing.ticketingId = org;
                return View(interTrading);
            }
            else
            {
                return View(_context.InterTrading.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditCarrotFacility(Guid org, int id)
        {
            if (id == 0)
            {
                CarrotFacility carrotFacility = new CarrotFacility();
                //ticketing.ticketingId = org;
                return View(carrotFacility);
            }
            else
            {
                return View(_context.CarrotFacility.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

    }
}