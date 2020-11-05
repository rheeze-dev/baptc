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
            var repair = _context.Repair.OrderByDescending(x => x.RequestNumber).ToList();
            return Json(new { data = repair });
        }

        // GET: api/Security/GetSecurityInspectionReport
        [HttpGet("GetSecurityInspectionReport")]
        public IActionResult GetSecurityInspectionReport([FromRoute]Guid organizationId)
        {
            var securityInspectionReport = _context.SecurityInspectionReport.OrderByDescending(x => x.ControlNumber).ToList();
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
            
            if (id == 0)
            {
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
                var currentRepair = _context.Repair.Where(x => x.Id == id).FirstOrDefault();
                if (currentRepair.PlateNumber != model["PlateNumber"].ToString() || currentRepair.Location != model["Location"].ToString() || currentRepair.Destination != model["Destination"].ToString() || currentRepair.DriverName != model["DriverName"].ToString() || currentRepair.RepairDetails != model["RepairDetails"].ToString() || currentRepair.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    var four = "";
                    var five = "";
                    var six = "";
                    if (currentRepair.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "PlateNumber = " + currentRepair.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentRepair.Location != model["Location"].ToString())
                    {
                        two = " Location = " + currentRepair.Location + " - " + model["Location"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentRepair.Destination != model["Destination"].ToString())
                    {
                        three = " Destination = " + currentRepair.Destination + " - " + model["Destination"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentRepair.DriverName != model["DriverName"].ToString())
                    {
                        four = " Driver name = " + currentRepair.DriverName + " - " + model["DriverName"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentRepair.RepairDetails != model["RepairDetails"].ToString())
                    {
                        five = " Repair details = " + currentRepair.RepairDetails + " - " + model["RepairDetails"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentRepair.Remarks != model["Remarks"].ToString())
                    {
                        six = " Remarks = " + currentRepair.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    var datas = one + two + three + four + five + six;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Vehicle repair",
                        EditedBy = info.FullName,
                        ControlNumber = currentRepair.Id
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                //currentRepair.Id = id;
                //currentRepair.RequestNumber = Convert.ToInt32(model["RequestNumber"].ToString());
                currentRepair.PlateNumber = model["PlateNumber"].ToString();
                currentRepair.Destination = model["Destination"].ToString();
                currentRepair.DriverName = model["DriverName"].ToString();
                currentRepair.Location = model["Location"].ToString();
                currentRepair.RepairDetails = model["RepairDetails"].ToString();
                currentRepair.Remarks = model["Remarks"].ToString();
                currentRepair.RequesterName = info.FullName;
                _context.Repair.Update(currentRepair);
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
            var getLastControlNumber = _context.SecurityInspectionReport.OrderByDescending(x => x.ControlNumber).Select(x => x.ControlNumber).FirstOrDefault();
      
            if (id == 0)
            {
                SecurityInspectionReport securityInspectionReport = new SecurityInspectionReport
                {
                    Location = model["Location"].ToString(),
                    Remarks = model["Remarks"].ToString(),
                    Action = model["Action"].ToString()
                };

                securityInspectionReport.Date = DateTime.Now;
                securityInspectionReport.Inspector = info.FullName;
                if (getLastControlNumber == null)
                {
                    securityInspectionReport.ControlNumber = 1;
                }
                else
                {
                    securityInspectionReport.ControlNumber = getLastControlNumber.Value + 1;
                }
                _context.SecurityInspectionReport.Add(securityInspectionReport);
            }
            else
            {
                SecurityInspectionReport securityInspection = _context.SecurityInspectionReport.Where(x => x.Id == id).FirstOrDefault();
                if (securityInspection.Location != model["Location"].ToString() || securityInspection.Action != model["Action"].ToString() || securityInspection.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    if (securityInspection.Location != model["Location"].ToString())
                    {
                        one = "Location = " + securityInspection.Location + " - " + model["Location"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (securityInspection.Action != model["Action"].ToString())
                    {
                        two = " Action = " + securityInspection.Action + " - " + model["Action"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (securityInspection.Remarks != model["Remarks"].ToString())
                    {
                        three = " Remarks = " + securityInspection.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    var datas = one + two + three;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Security inspection report",
                        EditedBy = info.FullName,
                        ControlNumber = securityInspection.ControlNumber.Value
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                securityInspection.Inspector = info.FullName;
                securityInspection.Location = model["Location"].ToString();
                securityInspection.Remarks = model["Remarks"].ToString();
                securityInspection.Action = model["Action"].ToString();
                _context.SecurityInspectionReport.Update(securityInspection);
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

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = repair.PlateNumber,
                Origin = "Repair",
                Name = repair.DriverName,
                DeletedBy = info.FullName,
                Remarks = repair.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Security/DeleteSecurityInspectionReport
        [HttpDelete("SecurityInspectionReport/{id}")]
        public async Task<IActionResult> DeleteSecurityInspectionReport([FromRoute] int id)
        {
            SecurityInspectionReport securityInspectionReport = _context.SecurityInspectionReport.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(securityInspectionReport);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Security",
                Name = "",
                DeletedBy = info.FullName,
                Remarks = securityInspectionReport.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }
    }
}