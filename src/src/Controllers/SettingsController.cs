﻿using System;
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

        //public IActionResult ConfigUserRoles(int userId)
        //{
        //    //ApplicationUser appUser = await _userManager.GetUserAsync(User);
        //    //Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
        //    //ViewData["org"] = org;
        //    //ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == userId).FirstOrDefault();
        //    return View(_context.ApplicationUser.Where(x => x.UserId == userId).FirstOrDefault());
        //}

    }
}