using System;
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
                DateAdded = Convert.ToDateTime(model["DateAdded"].ToString()),
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
        public async Task<IActionResult> DeleteTicketing([FromRoute] int id)
        {
            Roles roles = _context.Role.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(roles);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // POST: api/ModuleConfig/GetModules
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            List<ApplicationUser> currentUser = _context.ApplicationUser.ToList();
            var userModules = currentUser;
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
        public async Task<IActionResult> PostUserRole(UserRole userRole)
        {
            int id = userRole.Id;
            if (id == 0)
            {
                UserRole newUserRole = new UserRole
                {
                    Remarks = userRole.Remarks,
                    RoleId = userRole.RoleId,
                    UserId = userRole.UserId,
                    Modules = userRole.Modules,
                    DateAdded = DateTime.UtcNow
                };
                _context.UserRole.Add(newUserRole);
            }
            else
            {
                UserRole updateUserRole = _context.UserRole.Where(x => x.Id == id && x.UserId == userRole.UserId).FirstOrDefault();
                updateUserRole.Remarks = userRole.Remarks;
                updateUserRole.RoleId = userRole.RoleId;
                updateUserRole.Modules = userRole.Modules;
                _context.UserRole.Update(updateUserRole);
            }
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }
        // POST: api/Roles/GetUserRoleByUserID
        [HttpPost("GetUserRoleByUserID")]
        public IActionResult GetUserRoleByUserID(int userId)
        {
            UserRole userRole = _context.UserRole.Where(x => x.UserId == userId).FirstOrDefault();
            var userSelectedRoles = userRole.RoleId;
            List<Roles> listRole = _context.Role.ToList();
            if (userSelectedRoles != null)
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

    }
}