using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class ReportsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public ReportsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        public async Task<IActionResult> TicketingReports(Guid org)
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

        public async Task<IActionResult> ParkingNumberReports(Guid org)
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

        public async Task<IActionResult> TotalReports(Guid org)
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

        public async Task<IActionResult> StallLeaserReports(Guid org)
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

        public async Task<IActionResult> TradersTruckReports(Guid org)
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

        public async Task<IActionResult> FarmersTruckReports(Guid org)
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

        public async Task<IActionResult> ShortTripReports(Guid org)
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

        public async Task<IActionResult> InterTradingReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingAndInterTrading"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> CarrotFacilityReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("TradingAndInterTrading"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> SecurityReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Security"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> RepairReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Repair"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> PriceCommodityReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("PriceCommodities"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> AccreditationReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Accreditation"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> SettingsReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> DailyBuyersReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> DailyFarmersReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> DailyFacilitatorsReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TotalBuyersReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TotalFarmersReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> TotalFacilitatorsReports(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

    }
}