using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers
{
    [Authorize]
    public class ConfigController : BaseDotnetDeskController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfigController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var info = await _userManager.GetUserAsync(User);
            if (!this.IsHaveEnoughAccessRight())
            {
                return NotFound();
            }

            //UserRole userRoles = _context.UserRole.;
            //_context.Ticketing.OrderByDescending(x => x.controlNumber).Select(x => x.controlNumber).FirstOrDefault();
            //var ticketing = _context.Ticketing.OrderBy(x => x.timeIn).ToList();
            var roleId = _context.UserRole.OrderByDescending(x => x.RoleId).Select(x => x.RoleId).FirstOrDefault();
            //StallLease stallLease = _context.StallLease.Where(x => x.PlateNumber1 == ticketing.plateNumber || x.PlateNumber2 == ticketing.plateNumber).FirstOrDefault();

            if (roleId == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            //if (appUser.IsSuperAdmin)
            //{
            //    var orgList = _context.Organization.Where(x => x.organizationOwnerId.Equals(appUser.Id)).ToList();

            //    return View(orgList);
            //}
            //else if (appUser.IsSupportAgent)
            //{
            //    var supportAgent = _context.SupportAgent.Where(x => x.applicationUserId.Equals(appUser.Id)).FirstOrDefault();
            //    var orgList = _context.Organization.Where(x => x.organizationId.Equals(supportAgent.organizationId)).ToList();

            //    return View(orgList);
            //}
            //else if (appUser.IsSupportEngineer)
            //{
            //    var supportEngineer = _context.SupportEngineer.Where(x => x.applicationUserId.Equals(appUser.Id)).FirstOrDefault();
            //    var orgList = _context.Organization.Where(x => x.organizationId.Equals(supportEngineer.organizationId)).ToList();

            //    return View(orgList);
            //}
            //if (appUser.IsCustomer)
            //{
            //    var contact = _context.Contact.Where(x => x.applicationUserId.Equals(appUser.Id)).FirstOrDefault();
            //    var customer = _context.Customer.Where(x => x.customerId.Equals(contact.customerId)).FirstOrDefault();
            //    var orgList = _context.Organization.Where(x => x.organizationId.Equals(customer.organizationId)).ToList();

            //    return View(orgList);
            //}
            if (appUser.IsSuperAdmin)
            {
                var orgList = _context.Organization.Where(x => x.organizationOwnerId.Equals(appUser.Id)).ToList();

                return View(orgList);
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> IndexMobile()
        {
            var info = await _userManager.GetUserAsync(User);
            if (!this.IsHaveEnoughAccessRight())
            {
                return NotFound();
            }

            var roleId = _context.UserRole.OrderByDescending(x => x.RoleId).Select(x => x.RoleId).FirstOrDefault();

            if (roleId == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.GetUserAsync(User);
   
            if (appUser.IsSuperAdmin)
            {
                var orgList = _context.Organization.Where(x => x.organizationOwnerId.Equals(appUser.Id)).ToList();

                return View(orgList);
            }
            else
            {
                return View();
            }

        }

        public async Task<IActionResult> Organization()
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            return View(appUser);
        }

        public async Task<IActionResult> AddEditOrganization(Guid id)
        {
            
            if (Guid.Empty == id)
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(User);
                Organization org = new Organization();
                org.organizationOwnerId = appUser.Id;
                return View(org);
            }
            else
            {
                return View(_context.Organization.Where(x => x.organizationId.Equals(id)).FirstOrDefault());
            }

        }

        public async Task<IActionResult> UserProfile()
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            return View(appUser);
        }

        public async Task<IActionResult> PersonalProfile()
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            return View(appUser);
        }
    }
}