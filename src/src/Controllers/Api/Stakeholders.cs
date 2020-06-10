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
    [Route("api/Stakeholders")]
    //[Authorize]
    public class StakeholdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public StakeholdersController(ApplicationDbContext context,
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

        // GET: api/Stakeholders/GetStakeholders
        [HttpGet("GetBuyers")]
        public IActionResult GetBuyers([FromRoute]Guid organizationId)
        {
            var dailyBuyers = _context.DailyBuyers.OrderByDescending(x => x.Date).ToList();
            return Json(new { data = dailyBuyers });
        }

        // GET: api/Stakeholders/GetTotalBuyers
        [HttpGet("GetTotalBuyers")]
        public IActionResult GetTotalBuyers([FromRoute]Guid organizationId)
        {
            var totalBuyers = _context.TotalBuyers.OrderByDescending(x => x.Date).ToList();
            return Json(new { data = totalBuyers });
        }

        // GET: api/Stakeholders/GetFarmers
        [HttpGet("GetFarmers")]
        public IActionResult GetFarmers([FromRoute]Guid organizationId)
        {
            var dailyFarmers = _context.DailyFarmers.OrderByDescending(x => x.Date).ToList();
            return Json(new { data = dailyFarmers });
        }

        // GET: api/Stakeholders/GetTotalFarmers
        [HttpGet("GetTotalFarmers")]
        public IActionResult GetTotalFarmers([FromRoute]Guid organizationId)
        {
            var totalFarmers = _context.TotalFarmers.OrderByDescending(x => x.Date).ToList();
            return Json(new { data = totalFarmers });
        }

        // GET: api/Stakeholders/GetFacilitators
        [HttpGet("GetFacilitators")]
        public IActionResult GetFacilitators([FromRoute]Guid organizationId)
        {
            var dailyFacilitators = _context.DailyFacilitators.OrderByDescending(x => x.Date).ToList();
            return Json(new { data = dailyFacilitators });
        }

        // GET: api/Stakeholders/GetTotalFacilitators
        [HttpGet("GetTotalFacilitators")]
        public IActionResult GetTotalFacilitators([FromRoute]Guid organizationId)
        {
            var totalFacilitators = _context.TotalFacilitators.OrderByDescending(x => x.Date).ToList();
            return Json(new { data = totalFacilitators });
        }

        //// POST: api/Security
        //[HttpPost]
        //public async Task<IActionResult> PostBuyers([FromBody] JObject model)
        //{
        //    int id = 0;
        //    var getLastRequestNumber = _context.Repair.OrderByDescending(x => x.RequestNumber).Select(x => x.RequestNumber).FirstOrDefault();
        //    var info = await _userManager.GetUserAsync(User);
        //    id = Convert.ToInt32(model["Id"].ToString());
        //    DailyBuyers dailyBuyers = new DailyBuyers
        //    {
        //        Date = DateTime.Now,
        //        PlateNumber = model["PlateNumber"].ToString(),
        //        Destination = model["Destination"].ToString(),
        //        DriverName = model["DriverName"].ToString(),
        //        //RequesterName = model["RequesterName"].ToString(),
        //        Location = model["Location"].ToString(),
        //        RepairDetails = model["RepairDetails"].ToString(),
        //        Remarks = model["Remarks"].ToString()
        //    };

        //    if (id == 0)
        //    {
        //        if (getLastRequestNumber == null)
        //        {
        //            repair.RequestNumber = 1;
        //        }
        //        else
        //        {
        //            repair.RequestNumber = getLastRequestNumber + 1;
        //        }
        //        repair.RequesterName = info.FullName;
        //        _context.Repair.Add(repair);
        //    }
        //    else
        //    {
        //        repair.Id = id;
        //        repair.RequestNumber = Convert.ToInt32(model["RequestNumber"].ToString());
        //        repair.RequesterName = info.FullName;
        //        _context.Repair.Update(repair);
        //    }
        //    await _context.SaveChangesAsync();
        //    return Json(new { success = true, message = "Successfully Saved!" });
        //}

        //// POST: api/Security/PostSecurityInspectionReport
        //[HttpPost("PostSecurityInspectionReport")]
        //public async Task<IActionResult> PostSecurityInspectionReport([FromBody] JObject model)
        //{
        //    int id = 0;
        //    var info = await _userManager.GetUserAsync(User);
        //    id = Convert.ToInt32(model["Id"].ToString());
        //    SecurityInspectionReport securityInspectionReport = new SecurityInspectionReport
        //    {
        //        Date = DateTime.Now,
        //        Location = model["Location"].ToString(),
        //        Remarks = model["Remarks"].ToString(),
        //        Action = model["Action"].ToString()
        //    };
        //    if (id == 0)
        //    {
        //        securityInspectionReport.Inspector = info.FullName;
        //        _context.SecurityInspectionReport.Add(securityInspectionReport);
        //    }
        //    else
        //    {
        //        securityInspectionReport.Id = id;
        //        securityInspectionReport.Inspector = info.FullName;
        //        _context.SecurityInspectionReport.Update(securityInspectionReport);
        //    }
        //    await _context.SaveChangesAsync();
        //    return Json(new { success = true, message = "Successfully Saved!" });
        //}

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