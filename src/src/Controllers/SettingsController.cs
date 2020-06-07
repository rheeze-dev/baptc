using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
using src.Services;

namespace src.Controllers
{

    [Authorize]
    public class SettingsController : BaseDotnetDeskController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public SettingsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        //public class SettingsController : Controller
        //{
        //private readonly ApplicationDbContext _context;

        //public SettingsController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        public async Task<IActionResult> Roles(Guid org)
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

        public async Task<IActionResult> RolesMobile(Guid org)
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

        public async Task<IActionResult> ParkingNumbers(Guid org)
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

        public async Task<IActionResult> ParkingNumbersMobile(Guid org)
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

        public async Task<IActionResult> Addresses(Guid org)
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

        public async Task<IActionResult> AddressesMobile(Guid org)
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

        public async Task<IActionResult> Commodities(Guid org)
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

        public async Task<IActionResult> CommoditiesMobile(Guid org)
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

        public async Task<IActionResult> UserRoles(Guid org)
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(appUser);
        }

        public async Task<IActionResult> UserRolesMobile(Guid org)
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("Settings"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(appUser);
        }

        public async Task<IActionResult> TicketingPrice(Guid org)
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

        public async Task<IActionResult> TicketingPriceMobile(Guid org)
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

        public IActionResult AddEditTicketingPrice(Guid org, int id)
        {
            if (id == 0)
            {
                TicketingPrice ticketingPrice = new TicketingPrice();
                //ticketing.ticketingId = org;
                return View(ticketingPrice);
            }
            else
            {
                return View(_context.TicketingPrice.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewTicketingPrice(Guid org, int id)
        {
            if (id == 0)
            {
                TicketingPrice ticketingPrice = new TicketingPrice();
                //ticketing.ticketingId = org;
                return View(ticketingPrice);
            }
            else
            {
                return View(_context.TicketingPrice.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditRoles(Guid org, int id)
        {
            if (id == 0)
            {
                Roles roles = new Roles();
                //ticketing.ticketingId = org;
                return View(roles);
            }
            else
            {
                return View(_context.Role.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewRoles(Guid org, int id)
        {
            if (id == 0)
            {
                Roles roles = new Roles();
                //ticketing.ticketingId = org;
                return View(roles);
            }
            else
            {
                return View(_context.Role.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ConfigRoles(Guid org, int id)
        {
            if (id == 0)
            {
                Roles roles = new Roles();
                //ticketing.ticketingId = org;
                return View(roles);
            }
            else
            {
                return View(_context.Role.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditParkingNumbers(Guid org, int id)
        {
            if (id == 0)
            {
                ParkingNumbers parkingNumbers = new ParkingNumbers();
                //ticketing.ticketingId = org;
                return View(parkingNumbers);
            }
            else
            {
                return View(_context.ParkingNumbers.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewParkingNumbers(Guid org, int id)
        {
            if (id == 0)
            {
                ParkingNumbers parkingNumbers = new ParkingNumbers();
                //ticketing.ticketingId = org;
                return View(parkingNumbers);
            }
            else
            {
                return View(_context.ParkingNumbers.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditAddresses(Guid org, int id)
        {
            if (id == 0)
            {
                Addresses addresses = new Addresses();
                //ticketing.ticketingId = org;
                return View(addresses);
            }
            else
            {
                return View(_context.Addresses.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewAddresses(Guid org, int id)
        {
            if (id == 0)
            {
                Addresses addresses = new Addresses();
                //ticketing.ticketingId = org;
                return View(addresses);
            }
            else
            {
                return View(_context.Addresses.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult AddEditCommodities(Guid org, int id)
        {
            if (id == 0)
            {
                Commodities commodities = new Commodities();
                //ticketing.ticketingId = org;
                return View(commodities);
            }
            else
            {
                return View(_context.Commodities.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ViewCommodities(Guid org, int id)
        {
            if (id == 0)
            {
                Commodities commodities = new Commodities();
                //ticketing.ticketingId = org;
                return View(commodities);
            }
            else
            {
                return View(_context.Commodities.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ConfigRolesMobile(Guid org, int id)
        {
            if (id == 0)
            {
                Roles roles = new Roles();
                //ticketing.ticketingId = org;
                return View(roles);
            }
            else
            {
                return View(_context.Role.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }

        }

        public IActionResult ConfigUserRoles(Guid org, int userId)
        {
            if (userId == 0)
            {
                ApplicationUser userRoles = new ApplicationUser();
                //ticketing.ticketingId = org;
                return View(userRoles);
            }
            else
            {
                ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                //UserRole userRole = new UserRole();
                UserRole userRole = _context.UserRole.Where(x => x.UserId == userId).FirstOrDefault();
                if (userRole == null)
                {
                    userRole = new UserRole();
                }
                var TubleList = new Tuple<ApplicationUser, UserRole>(applicationUser, userRole);
                return View(TubleList);
            }

        }

        public IActionResult ConfigUserRolesMobile(Guid org, int userId)
        {
            if (userId == 0)
            {
                ApplicationUser userRoles = new ApplicationUser();
                //ticketing.ticketingId = org;
                return View(userRoles);
            }
            else
            {
                ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId.Equals(userId)).FirstOrDefault();
                //UserRole userRole = new UserRole();
                UserRole userRole = _context.UserRole.Where(x => x.UserId == userId).FirstOrDefault();
                if (userRole == null)
                {
                    userRole = new UserRole();
                }
                var TubleList = new Tuple<ApplicationUser, UserRole>(applicationUser, userRole);
                return View(TubleList);
            }

        }

    }
}