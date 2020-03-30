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
        private readonly SignInManager<ApplicationUser> _signInManager;



        public SecurityController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        // GET: api/Security
        [HttpGet("{organizationId}")]
        public IActionResult GetSecurityRepairCheck([FromRoute]Guid organizationId)
        {
            var repair = _context.Repair.ToList();
            return Json(new { data = repair });
        }

        // GET: api/Security/GetSecurityInspectionReport
        [HttpGet("GetSecurityInspectionReport")]
        public IActionResult GetSecurityInspectionReport([FromRoute]Guid organizationId)
        {
            var securityInspectionReport = _context.SecurityInspectionReport.ToList();
            return Json(new { data = securityInspectionReport });
        }

        // POST: api/Security
        [HttpPost]
        public async Task<IActionResult> PostSecurityRepairCheck([FromBody] JObject model)
        {
            int id = 0;
            var getLastRequestNumber = _context.Repair.OrderByDescending(x => x.RequestNumber).Select(x => x.RequestNumber).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            Repair repair = new Repair
            {
                Date = DateTime.Now,
                PlateNumber = model["PlateNumber"].ToString(),
                Destination = model["Destination"].ToString(),
                DriverName = model["DriverName"].ToString(),
                //RequesterName = model["RequesterName"].ToString(),
                Location = model["Location"].ToString(),
                RepairDetails = model["RepairDetails"].ToString(),
                Remarks = model["Remarks"].ToString()
            };
            
            if (id == 0)
            {
                if (getLastRequestNumber == null)
                {
                    repair.RequestNumber = 1;
                }
                else
                {
                    repair.RequestNumber = getLastRequestNumber + 1;
                }
                repair.RequesterName = info.FullName;
                _context.Repair.Add(repair);
            }
            else
            {
                repair.Id = id;
                repair.RequestNumber = Convert.ToInt32(model["RequestNumber"].ToString());
                repair.RequesterName = info.FullName;
                _context.Repair.Update(repair);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Security/PostSecurityInspectionReport
        [HttpPost("PostSecurityInspectionReport")]
        public async Task<IActionResult> PostSecurityInspectionReport([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            SecurityInspectionReport securityInspectionReport = new SecurityInspectionReport
            {
                Date = DateTime.Now,
                Location = model["Location"].ToString(),
                Remarks = model["Remarks"].ToString(),
                Action = model["Action"].ToString()
            };
            if (id == 0)
            {
                securityInspectionReport.Inspector = info.FullName;
                _context.SecurityInspectionReport.Add(securityInspectionReport);
            }
            else
            {
                securityInspectionReport.Id = id;
                securityInspectionReport.Inspector = info.FullName;
                _context.SecurityInspectionReport.Update(securityInspectionReport);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Security/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSecurityRepairCheck([FromRoute] int id)
        {
            Repair repair = _context.Repair.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(repair);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Security/DeleteSecurityInspectionReport
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