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
    public class AccreditationController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public AccreditationController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        public async Task<IActionResult> InterTraders(Guid org)
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

        public async Task<IActionResult> PackersAndPorters(Guid org)
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

        public async Task<IActionResult> Buyers(Guid org)
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

        public async Task<IActionResult> MarketFacilitators(Guid org)
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

        public async Task<IActionResult> IndividualFarmers(Guid org)
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

        public IActionResult AddEditInterTraders(Guid org, int id)
        {
            if (id == 0)
            {
                InterTraders interTraders = new InterTraders();
                //ticketing.ticketingId = org;
                return View(interTraders);
            }
            else
            {
                return View(_context.AccreditedInterTraders.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditPackersAndPorters(Guid org, int id)
        {
            if (id == 0)
            {
                PackersAndPorters packersAndPorters = new PackersAndPorters();
                //ticketing.ticketingId = org;
                return View(packersAndPorters);
            }
            else
            {
                return View(_context.AccreditedPackersAndPorters.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditBuyers(Guid org, int id)
        {
            if (id == 0)
            {
                Buyers buyers = new Buyers();
                //ticketing.ticketingId = org;
                return View(buyers);
            }
            else
            {
                return View(_context.AccreditedBuyers.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditMarketFacilitators(Guid org, int id)
        {
            if (id == 0)
            {
                MarketFacilitators marketFacilitators = new MarketFacilitators();
                //ticketing.ticketingId = org;
                return View(marketFacilitators);
            }
            else
            {
                return View(_context.AccreditedMarketFacilitators.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditIndividualFarmers(Guid org, int id)
        {
            if (id == 0)
            {
                IndividualFarmers individualFarmers = new IndividualFarmers();
                //ticketing.ticketingId = org;
                return View(individualFarmers);
            }
            else
            {
                return View(_context.AccreditedIndividualFarmers.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

    }
}