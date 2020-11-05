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
using src.Enum;
using src.Models;
using src.Services;

namespace src.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/ModuleConfig")]
    //[Authorize]
    public class ModuleConfigController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ModuleConfigController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/ModuleConfig
        [HttpGet("{organizationId}")]
        public IActionResult GetModuleConfig([FromRoute]Guid organizationId)
        {
            var moduleConfig = _context.Role.ToList();
            return Json(new { data = moduleConfig });
        }


        // POST: api/ModuleConfig/GetModules
        [HttpPost("GetModules")]
        public IActionResult GetModules(int roleId)
        {
            Roles currentRole = _context.Role.Where(role => role.Id == roleId).FirstOrDefault();
            var roleModules = currentRole.Module;
            List<Module> listModule = _context.Modules.ToList();
            
            if (roleModules != null)
            {
                var query = from val in roleModules.Split(',')
                            select (val);
                foreach (var item in listModule)
                {
                    bool containsItem = query.Any(x => x == item.Name);
                    if (containsItem)
                        item.Selected = true;
                }
            }

            return Json(new { data = listModule });
        }

        // POST: api/ModuleConfig/GetUserModuleByUserID
        [HttpPost("GetUserModuleByUserID")]
        public IActionResult GetUserModuleByUserID(int userId)
        {
            UserRole userRole = _context.UserRole.Where(x => x.UserId == userId).FirstOrDefault();
            var userSelectedModules = "";
            if (userRole != null && userRole.Modules != null)
            {
                userSelectedModules = userRole.Modules;
            }
            List<Module> listRole = _context.Modules.ToList();

            if (userSelectedModules != "")
            {
                var query = from val in userSelectedModules.Split(',')
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

        [HttpPost("UpdateRoleModules")]
        public async Task<IActionResult> UpdateRoleModules(int roleId,string selectedModule)
        {
            var info = await _userManager.GetUserAsync(User);
            Roles currentRole = _context.Role.Where(role => role.Id == roleId).FirstOrDefault();
            if (currentRole.Module != selectedModule)
            {
                var module = "";
                EditedDatas editedDatas = new EditedDatas
                {
                    DateEdited = DateTime.Now,
                    Origin = "Roles/Config",
                    EditedBy = info.FullName,
                    ControlNumber = currentRole.Id
                };
                module = "Module = " + currentRole.Module + " - " + selectedModule + ";";
                editedDatas.EditedData = module;
                editedDatas.Remarks = currentRole.Remarks;
                _context.EditedDatas.Add(editedDatas);
            }
            currentRole.Module = selectedModule;
            currentRole.Modifier = info.FullName;
            _context.Role.Update(currentRole);
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/ModuleConfig/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModuleConfig([FromRoute] int id)
        {
            Roles roles = _context.Role.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(roles);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }
    }
}