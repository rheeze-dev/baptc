using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using src.Data;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class TicketingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public TicketingController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        public async Task<IActionResult> TicketingOut(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TicketingOutMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ViewAllTickets(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TicketingIn(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TicketingInMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> GatePass(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> GatePassMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Ticketing"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditIn(Guid org, Guid id)
        {
            var listParking = _context.ParkingNumbers.Where(x => x.Selected == false).ToList();
            var parkingNos = new List<SelectListItem>();
            foreach (var item in listParking)
            {
                parkingNos.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Name
                });
            }
            parkingNos.Insert(0, new SelectListItem()
            {
                Text = "Please Select",
                Value = ""
            });
            if (id == Guid.Empty)
            {
                Ticketing ticketing = new Ticketing();
                ticketing.parkingList = parkingNos;
                //ticketing.ticketingId = org;
                return View(ticketing);
            }
            else
            {
                Ticketing ticketingUpdate = _context.Ticketing.Where(x => x.ticketingId.Equals(id)).FirstOrDefault();
                ticketingUpdate.parkingList = parkingNos;
                return View(ticketingUpdate);
            }

        }

        public IActionResult ViewTicketingIn(Guid org, Guid id)
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

        public IActionResult Finish(Guid org, Guid id)
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

        public IActionResult Debit(Guid org, Guid id)
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
                StallLease stallLease = new StallLease();
                //customer.organizationId = org;
                return View(stallLease);
            }
            else
            {
                return View(_context.StallLease.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
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