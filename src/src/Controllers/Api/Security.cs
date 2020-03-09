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
    [Route("api/Security")]
    //[Authorize]
    public class SecurityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public SecurityController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/Inspector
        [HttpGet("{organizationId}")]
        public IActionResult GetSecurityRepairCheck([FromRoute]Guid organizationId)
        {
            var securityRepairCheck = _context.SecurityRepairCheck.ToList();
            return Json(new { data = securityRepairCheck });
        }

        // GET: api/Inspector/GetSecurityInspectionReport
        [HttpGet("GetSecurityInspectionReport")]
        public IActionResult GetSecurityInspectionReport([FromRoute]Guid organizationId)
        {
            var securityInspectionReport = _context.SecurityInspectionReport.ToList();
            return Json(new { data = securityInspectionReport });
        }

        // POST: api/Inspector
        [HttpPost]
        public async Task<IActionResult> PostSecurityRepairCheck([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            SecurityRepairCheck securityRepairCheck = new SecurityRepairCheck
            {
                Date = Convert.ToDateTime(model["Date"].ToString()),
                Name = model["Name"].ToString(),
                PlateNumber = model["PlateNumber"].ToString(),
                RepairDetails = model["RepairDetails"].ToString(),
                Remarks = model["Remarks"].ToString()
            };
            if (id == 0)
            {
                _context.SecurityRepairCheck.Add(securityRepairCheck);
            }
            else
            {
                securityRepairCheck.Id = id;
                _context.SecurityRepairCheck.Update(securityRepairCheck);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostSecurityInspectionReport
        [HttpPost("PostSecurityInspectionReport")]
        public async Task<IActionResult> PostSecurityInspectionReport([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            SecurityInspectionReport securityInspectionReport = new SecurityInspectionReport
            {
                Date = Convert.ToDateTime(model["Date"].ToString()),
                Location = model["Location"].ToString(),
                Remarks = model["Remarks"].ToString()
            };
            if (id == 0)
            {
                _context.SecurityInspectionReport.Add(securityInspectionReport);
            }
            else
            {
                securityInspectionReport.Id = id;
                _context.SecurityInspectionReport.Update(securityInspectionReport);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Inspector/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSecurityRepairCheck([FromRoute] int id)
        {
            SecurityRepairCheck securityRepairCheck = _context.SecurityRepairCheck.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(securityRepairCheck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteSecurityInspectionReport
        [HttpDelete("SecurityInspectionReport/{id}")]
        public async Task<IActionResult> DeleteSecurityInspectionReport([FromRoute] int id)
        {
            SecurityInspectionReport securityInspectionReport = _context.SecurityInspectionReport.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(securityInspectionReport);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}