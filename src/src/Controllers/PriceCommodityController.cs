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
    public class PriceCommodityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public PriceCommodityController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        //public async Task<IActionResult> Index(Guid org)
        //{
        //    if (org == Guid.Empty)
        //    {
        //        return NotFound();
        //    }
        //    ApplicationUser appUser = await _userManager.GetUserAsync(User);
        //    var listModule = _securityService.ListModule(appUser);
        //    if (!listModule.Contains("PriceCommodities"))
        //    {
        //        return NotFound();
        //    }
        //    Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
        //    ViewData["org"] = org;
        //    return View(organization);
        //}

        public async Task<IActionResult> Price(Guid org)
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

        public async Task<IActionResult> Graph(Guid org)
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

        public IActionResult AddEditPrice(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                PriceCommodity priceCommodity = new PriceCommodity();
                //priceCommodity.organizationId = org;
                return View(priceCommodity);
            }
            else
            {
                return View(_context.PriceCommodity.Where(x => x.priceCommodityId.Equals(id)).FirstOrDefault());
            }

        }
    }
}