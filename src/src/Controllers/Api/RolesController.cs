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
    }
}