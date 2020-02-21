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

        public IActionResult Customer(Guid cust)
        {
            if (cust == Guid.Empty)
            {
                return NotFound();
            }
            Customer customer = _context.Customer.Where(x => x.customerId.Equals(cust)).FirstOrDefault();
            ViewData["cust"] = cust;
            return View(customer);
        }

        public IActionResult AddEditTradersTruck(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Ticket ticket = new Ticket();
                ticket.organizationId = org;

                IList<Product> products = _context.Product.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.productId = new SelectList(products, "productId", "productName");

                IList<SupportAgent> agents = _context.SupportAgent.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.supportAgentId = new SelectList(agents, "supportAgentId", "supportAgentName");

                IList<SupportEngineer> engineers = _context.SupportEngineer.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.supportEngineerId = new SelectList(engineers, "supportEngineerId", "supportEngineerName");

                IList<Contact> contacts = _context.Contact
                    .Where(x => x.customer.organizationId.Equals(org)).ToList();
                ViewBag.contactId = new SelectList(contacts, "contactId", "contactName");

                return View(ticket);
            }
            else
            {
                Ticket ticket = _context.Ticket.Where(x => x.ticketId.Equals(id)).FirstOrDefault();

                IList<Product> products = _context.Product.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.productId = new SelectList(products, "productId", "productName", ticket.productId);

                IList<SupportAgent> agents = _context.SupportAgent.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.supportAgentId = new SelectList(agents, "supportAgentId", "supportAgentName", ticket.supportAgentId);

                IList<SupportEngineer> engineers = _context.SupportEngineer.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.supportEngineerId = new SelectList(engineers, "supportEngineerId", "supportEngineerName", ticket.supportEngineerId);

                IList<Contact> contacts = _context.Contact
                    .Where(x => x.customer.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.contactId = new SelectList(contacts, "contactId", "contactName", ticket.contactId);

                return View(ticket);
            }

        }

        public IActionResult AddEditFarmersTruck(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Ticket_FarmersTruck ticket = new Ticket_FarmersTruck();
                ticket.organizationId = org;

                IList<Product> products = _context.Product.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.productId = new SelectList(products, "productId", "productName");

                IList<SupportAgent> agents = _context.SupportAgent.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.supportAgentId = new SelectList(agents, "supportAgentId", "supportAgentName");

                IList<SupportEngineer> engineers = _context.SupportEngineer.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.supportEngineerId = new SelectList(engineers, "supportEngineerId", "supportEngineerName");

                IList<Contact> contacts = _context.Contact
                    .Where(x => x.customer.organizationId.Equals(org)).ToList();
                ViewBag.contactId = new SelectList(contacts, "contactId", "contactName");

                return View(ticket);
            }
            else
            {
                Ticket ticket = _context.Ticket.Where(x => x.ticketId.Equals(id)).FirstOrDefault();

                IList<Product> products = _context.Product.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.productId = new SelectList(products, "productId", "productName", ticket.productId);

                IList<SupportAgent> agents = _context.SupportAgent.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.supportAgentId = new SelectList(agents, "supportAgentId", "supportAgentName", ticket.supportAgentId);

                IList<SupportEngineer> engineers = _context.SupportEngineer.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.supportEngineerId = new SelectList(engineers, "supportEngineerId", "supportEngineerName", ticket.supportEngineerId);

                IList<Contact> contacts = _context.Contact
                    .Where(x => x.customer.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.contactId = new SelectList(contacts, "contactId", "contactName", ticket.contactId);

                return View(ticket);
            }

        }

        public IActionResult AddEditShortTrip(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                Ticket_ShortTrip ticket = new Ticket_ShortTrip();
                ticket.organizationId = org;

                IList<Product> products = _context.Product.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.productId = new SelectList(products, "productId", "productName");

                IList<SupportAgent> agents = _context.SupportAgent.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.supportAgentId = new SelectList(agents, "supportAgentId", "supportAgentName");

                IList<SupportEngineer> engineers = _context.SupportEngineer.Where(x => x.organizationId.Equals(org)).ToList();
                ViewBag.supportEngineerId = new SelectList(engineers, "supportEngineerId", "supportEngineerName");

                IList<Contact> contacts = _context.Contact
                    .Where(x => x.customer.organizationId.Equals(org)).ToList();
                ViewBag.contactId = new SelectList(contacts, "contactId", "contactName");

                return View(ticket);
            }
            else
            {
                Ticket ticket = _context.Ticket.Where(x => x.ticketId.Equals(id)).FirstOrDefault();

                IList<Product> products = _context.Product.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.productId = new SelectList(products, "productId", "productName", ticket.productId);

                IList<SupportAgent> agents = _context.SupportAgent.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.supportAgentId = new SelectList(agents, "supportAgentId", "supportAgentName", ticket.supportAgentId);

                IList<SupportEngineer> engineers = _context.SupportEngineer.Where(x => x.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.supportEngineerId = new SelectList(engineers, "supportEngineerId", "supportEngineerName", ticket.supportEngineerId);

                IList<Contact> contacts = _context.Contact
                    .Where(x => x.customer.organizationId.Equals(ticket.organizationId)).ToList();
                ViewBag.contactId = new SelectList(contacts, "contactId", "contactName", ticket.contactId);

                return View(ticket);
            }

        }

    }
}