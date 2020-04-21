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
    public class TradingInspectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public TradingInspectorController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        public async Task<IActionResult> TradersTruck(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TradersTruckMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> FarmersTruck(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> FarmersTruckMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ShortTrip(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ShortTripMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> PayParking(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingInspector"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public IActionResult AddEditTradersTruck(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                TradersTruck tradersTruck = new TradersTruck();
                //ticketing.ticketingId = org;
                return View(tradersTruck);
            }
            else
            {
                return View(_context.TradersTruck.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewTradersTruck(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                TradersTruck tradersTruck = new TradersTruck();
                //ticketing.ticketingId = org;
                return View(tradersTruck);
            }
            else
            {
                return View(_context.TradersTruck.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditFarmersTruck(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                FarmersTruck farmersTruck = new FarmersTruck();
                //ticketing.ticketingId = org;
                return View(farmersTruck);
            }
            else
            {
                return View(_context.FarmersTruck.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewFarmersTruck(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                FarmersTruck farmersTruck = new FarmersTruck();
                //ticketing.ticketingId = org;
                return View(farmersTruck);
            }
            else
            {
                return View(_context.FarmersTruck.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditShortTrip(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                ShortTrip shortTrip = new ShortTrip();
                //ticketing.ticketingId = org;
                return View(shortTrip);
            }
            else
            {
                return View(_context.ShortTrip.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }
        }

        public IActionResult ViewShortTrip(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                ShortTrip shortTrip = new ShortTrip();
                //ticketing.ticketingId = org;
                return View(shortTrip);
            }
            else
            {
                return View(_context.ShortTrip.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }
        }

        public IActionResult AddEditPayParking(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                PayParking payParking = new PayParking();
                //ticketing.ticketingId = org;
                return View(payParking);
            }
            else
            {
                return View(_context.PayParking.Where(x => x.ticketingId.Equals(id)).FirstOrDefault());
            }
        }

    }
}