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

        //public IActionResult Index(Guid org)
        //{
        //    if (org == Guid.Empty)
        //    {
        //        return NotFound();
        //    }
        //    Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
        //    ViewData["org"] = org;
        //    return View(organization);
        //}

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

        public IActionResult AddEditIn(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Customer customer = new Customer();
                customer.organizationId = org;
                return View(customer);
            }
            else
            {
                return View(_context.Customer.Where(x => x.customerId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditOut(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Customer_TicketingOut customer = new Customer_TicketingOut();
                customer.organizationId = org;
                return View(customer);
            }
            else
            {
                return View(_context.Customer.Where(x => x.customerId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult Detail(Guid customerId)
        {
            if (customerId == Guid.Empty)
            {
                return NotFound();
            }

            Customer customer = _context.Customer.Where(x => x.customerId.Equals(customerId)).FirstOrDefault();
            ViewData["org"] = customer.organizationId;
            return View(customer);
        }
        
    }
}