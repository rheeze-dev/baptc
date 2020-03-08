using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class TradingInspectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TradingInspectorController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult TradersTruck(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult FarmersTruck(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult ShortTrip(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditTradersTruck(Guid org, int id)
        {
            if (id == 0)
            {
                TradersTruck tradersTruck = new TradersTruck();
                //ticketing.ticketingId = org;
                return View(tradersTruck);
            }
            else
            {
                return View(_context.TradersTruck.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditFarmersTruck(Guid org, int id)
        {
            if (id == 0)
            {
                FarmersTruck farmersTruck = new FarmersTruck();
                //ticketing.ticketingId = org;
                return View(farmersTruck);
            }
            else
            {
                return View(_context.FarmersTruck.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditShortTrip(Guid org, int id)
        {
            if (id == 0)
            {
                ShortTrip shortTrip = new ShortTrip();
                //ticketing.ticketingId = org;
                return View(shortTrip);
            }
            else
            {
                return View(_context.ShortTrip.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

    }
}