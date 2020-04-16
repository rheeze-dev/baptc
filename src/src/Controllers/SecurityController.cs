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
    public class SecurityController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public SecurityController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        public async Task <IActionResult> SecurityInspectionReport(Guid org)
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

        public IActionResult AddEditSecurityInspectionReport(Guid org, int id)
        {
            if (id == 0)
            {
                SecurityInspectionReport securityInspectionReport = new SecurityInspectionReport();
                //ticketing.ticketingId = org;
                return View(securityInspectionReport);
            }
            else
            {
                return View(_context.SecurityInspectionReport.Where(x => x.Id.Equals(id)).FirstOrDefault());
            }
        }

    }
}