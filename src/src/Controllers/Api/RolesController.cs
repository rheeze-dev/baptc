﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using src.Data;
using src.Models;
using src.Services;

namespace src.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Roles")]
    //[Authorize]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RolesController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/Roles
        [HttpGet("{organizationId}")]
        public IActionResult GetRoles([FromRoute]Guid organizationId)
        {
            var roles = _context.Role.ToList();
            return Json(new { data = roles });
        }


        // POST: api/Roles
        [HttpPost]
        public async Task<IActionResult> PostRoles([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            Roles roles = new Roles
            {
                DateAdded = DateTime.Now,
                FullName = model["FullName"].ToString(),
                Remarks = model["Remarks"].ToString(),
                ShortName = model["ShortName"].ToString(),
                Name = model["ShortName"].ToString(),
            };
            if (id == 0)
            {
                _context.Role.Add(roles);
            }
            else
            {
                roles.Id = id;
                _context.Role.Update(roles);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Roles
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoles([FromRoute] int id)
        {
            Roles roles = _context.Role.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(roles);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles
        [HttpDelete("UserRoles/{id}")]
        public async Task<IActionResult> DeleteUserRoles([FromRoute] int id)
        {
            UserRole userRole = _context.UserRole.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(userRole);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // POST: api/ModuleConfig/GetModules
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            List<ApplicationUser> currentUser = _context.ApplicationUser.ToList();
            //var userModules = currentUser;
            List<ApplicationUser> listUser = _context.ApplicationUser.ToList();

            //if (userModules != null)
            //{
            //    var query = from val in userModules.Split(',')
            //                select (val);
            //    foreach (var item in listUser)
            //    {
            //        bool containsItem = query.Any(x => x == item.FullName);
            //        if (containsItem)
            //            item.Selected = true;
            //    }
            //}
            return Json(new { data = listUser });
        }
        //[HttpPost("ConfigUserRoles")]
        public IActionResult ConfigUserRole(int userId)
        {
            ApplicationUser applicationUser = _context.ApplicationUser.Where(user => user.UserId == userId).FirstOrDefault();

            return Json(new { data = applicationUser });

        }

        // POST: api/Roles/PostUserRole
        [HttpPost("PostUserRole")]
        public async Task<IActionResult> PostUserRole(ApplicationUser applicationUser, UserRole userRole)
        {
            int id = applicationUser.UserId;
            int userRoleId = userRole.Id;
            var info = await _userManager.GetUserAsync(User);
             
            ApplicationUser updateApplicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
                {
                    updateApplicationUser.RoleId = applicationUser.RoleId;
                    updateApplicationUser.Modules = applicationUser.Modules;
                    updateApplicationUser.DateModified = DateTime.Now;
                    updateApplicationUser.Modifier = info.FullName;
                if (applicationUser.RoleId == null)
                {
                    return Json(new { success = false, message = "Role field cannot be empty!" });
                }
                else if (applicationUser.RoleId.Contains(","))
                {
                    return Json(new { success = false, message = "Role field cannot be more than 1!" });
                }

                _context.ApplicationUser.Update(updateApplicationUser);
                }

            if (userRoleId == 0)
            {
                UserRole newUserRole = new UserRole
                {
                    RoleId = updateApplicationUser.RoleId,
                    UserId = updateApplicationUser.UserId,
                    Modules = updateApplicationUser.Modules
                };
                _context.UserRole.Add(newUserRole);
                }
                else
                {
                    UserRole updateUserRole = _context.UserRole.Where(x => x.UserId == id).FirstOrDefault();
                    {
                        updateUserRole.RoleId = applicationUser.RoleId;
                        updateUserRole.Modules = applicationUser.Modules;
      
                        _context.UserRole.Update(updateUserRole);
                    }
                }
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/GetUserRoleByUserID
        [HttpPost("GetUserRoleByUserID")]
        public IActionResult GetUserRoleByUserID(int userId)
        {
            UserRole userRole = _context.UserRole.Where(x => x.UserId == userId).FirstOrDefault();
            var userSelectedRoles = "";
            if (userRole != null && userRole.RoleId != null)
            {
                userSelectedRoles = userRole.RoleId;
            }
            List<Roles> listRole = _context.Role.ToList();

            if (userSelectedRoles != "")
            {
                var query = from val in userSelectedRoles.Split(',')
                            select (val);
                foreach (var item in listRole)
                {
                    bool containsItem = query.Any(x => x == item.Name);
                    if (containsItem)
                        item.Selected = true;
                }
            }
            return Json(new { data = listRole });
        }

        // POST: api/Roles/DeactivateUser
        [HttpPost("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            UserRole userRole = _context.UserRole.Where(x => x.UserId == id).FirstOrDefault();
            {
                userRole.RoleId = "Default";
                userRole.Modules = null;
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.RoleId = "Default";
                applicationUser.Modules = null;
            }

            _context.UserRole.Update(userRole);
            _context.ApplicationUser.Update(applicationUser);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account has been Deactivated!" });
        }

    }
}