using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    public class TicketingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult TicketingOut(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult TicketingIn(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult GatePass(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditIn(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Ticketing ticketing = new Ticketing();
                //ticketing.ticketingId = org;
                return View(ticketing);
            }
            else
            {
                return View(_context.Ticketing.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditOut(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Ticketing ticketing = new Ticketing();
                //customer.organizationId = org;
                return View(ticketing);
            }
            else
            {
                return View(_context.Ticketing.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditGatePass(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                GatePass gatePass = new GatePass();
                //customer.organizationId = org;
                return View(gatePass);
            }
            else
            {
                return View(_context.GatePass.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditNewGatePass(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Ticketing ticketing = new Ticketing();
                //customer.organizationId = org;
                return View(ticketing);
            }
            else
            {
                return View(_context.Ticketing.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

    }
}