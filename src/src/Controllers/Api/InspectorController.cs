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
    [Route("api/Inspector")]
    //[Authorize]
    public class InspectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public InspectorController(ApplicationDbContext context,
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
        public IActionResult GetTradersTruck([FromRoute]Guid organizationId)
        {
            var tradersTruck = _context.TradersTruck.ToList();
            return Json(new { data = tradersTruck });
        }

        // GET: api/Inspector/GetFarmersTruck
        [HttpGet("GetFarmersTruck")]
        public IActionResult GetFarmersTruck([FromRoute]Guid organizationId)
        {
            var farmersTruck = _context.FarmersTruck.ToList();
            return Json(new { data = farmersTruck });
        }

        // GET: api/Inspector/GetShortTrip
        [HttpGet("GetShortTrip")]
        public IActionResult GetShortTrip([FromRoute]Guid organizationId)
        {
            var shortTrip = _context.ShortTrip.ToList();
            return Json(new { data = shortTrip });
        }

        // POST: api/Inspector
        [HttpPost]
        public async Task<IActionResult> PostTradersTruck([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            TradersTruck tradersTruck = new TradersTruck
            {
                Date = Convert.ToDateTime(model["Date"].ToString()),
                TraderName = model["TraderName"].ToString(),
                PlateNumber = model["PlateNumber"].ToString(),
                EstimatedVolume = Convert.ToInt32(model["EstimatedVolume"].ToString()),
                Destination = model["Destination"].ToString()
            };
            if (id == 0)
            {
                _context.TradersTruck.Add(tradersTruck);
            }
            else
            {
                tradersTruck.Id = id;
                _context.TradersTruck.Update(tradersTruck);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostFarmersTruck
        [HttpPost("PostFarmersTruck")]
        public async Task<IActionResult> PostFarmersTruck([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            FarmersTruck farmersTruck = new FarmersTruck
            {
                Date = Convert.ToDateTime(model["Date"].ToString()),
                StallNumber = model["StallNumber"].ToString(),
                PlateNumber = model["PlateNumber"].ToString(),
                FarmersName = model["FarmersName"].ToString(),
                Organization = model["Organization"].ToString(),
                Commodity = model["Commodity"].ToString(),
                Volume = Convert.ToInt32(model["Volume"].ToString()),
                Barangay = model["Barangay"].ToString(),
                Province = model["Province"].ToString(),
                FacilitatorsName = model["FacilitatorsName"].ToString()
            };
            if (id == 0)
            {
                _context.FarmersTruck.Add(farmersTruck);
            }
            else
            {
                farmersTruck.Id = id;
                _context.FarmersTruck.Update(farmersTruck);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostShortTrip
        [HttpPost("PostShortTrip")]
        public async Task<IActionResult> PostShortTrip([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            ShortTrip shortTrip = new ShortTrip
            {
                TimeIn = Convert.ToDateTime(model["TimeIn"].ToString()),
                EstimatedVolume = Convert.ToInt32(model["EstimatedVolume"].ToString()),
                PlateNumber = model["PlateNumber"].ToString(),
                Commodity = model["Commodity"].ToString()
            };
            if (id == 0)
            {
                _context.ShortTrip.Add(shortTrip);
            }
            else
            {
                shortTrip.Id = id;
                _context.ShortTrip.Update(shortTrip);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Inspector/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTradersTruck([FromRoute] int id)
        {
            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(tradersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteFarmersTruck
        [HttpDelete("Farmers/{id}")]
        public async Task<IActionResult> DeleteFarmersTruck([FromRoute] int id)
        {
            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(farmersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteShortTrip
        [HttpDelete("ShortTrip/{id}")]
        public async Task<IActionResult> DeleteShortTrip([FromRoute] int id)
        {
            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(shortTrip);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }
    }
}