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
    public class DeletedDatasController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public DeletedDatasController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        public async Task<IActionResult> ExportDeletedDatas(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ExportDeletedDatasMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ViewDeletedDatas(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ViewEditedDatas(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ViewEditedDatasMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ViewDeletedDatasMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ResetDatas(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> ResetDatasMobile(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("DeletedDatas"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

    }
}