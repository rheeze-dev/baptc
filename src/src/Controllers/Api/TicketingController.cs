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
    [Route("api/Ticketing")]
    //[Authorize]
    public class TicketingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public TicketingController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/Ticketing
        [HttpGet("{organizationId}")]
        public IActionResult GetTicketing([FromRoute]Guid organizationId)
        {
            var ticketing = _context.Ticketing.OrderBy(x => x.timeIn).ToList();
            return Json(new { data = ticketing });
        }
        //[HttpGet("GetTicketOut")]
        //public IActionResult GetTicketOut([FromRoute]Guid organizationId)
        //{
        //    //var ticketing = _context.Ticketing.Where(x => x.timeOut == null).OrderBy(x => x.timeIn).ToList();
        //    var ticketing = _context.Ticketing.OrderBy(x => x.timeIn).ToList();
        //    return Json(new { data = ticketing });
        //}
        // GET: api/Ticketing/GetGatePass
        [HttpGet("GetGatePass")]
        public IActionResult GetGatePass([FromRoute]Guid organizationId)
        {
            var gatePass = _context.GatePass.ToList();
            return Json(new { data = gatePass });
        }

        // POST: api/Ticketing
        [HttpPost]
        public async Task<IActionResult> PostTicketing([FromBody] JObject model)
        {
            Guid objGuid = Guid.Empty;

            objGuid = Guid.Parse(model["ticketingId"].ToString());
            
            Ticketing ticketing = new Ticketing
            {
                timeIn = DateTime.Now,
                plateNumber = model["plateNumber"].ToString(),
                typeOfTransaction = model["typeOfTransaction"].ToString(),
                typeOfCar = model["typeOfCar"].ToString(),
                amount = null
            };

            TradersTruck tradersTruck = new TradersTruck
            {
                ticketingId = ticketing.ticketingId,
                TimeIn = DateTime.Now,
                PlateNumber = ticketing.plateNumber
            };

            FarmersTruck farmersTruck = new FarmersTruck
            {
                ticketingId = ticketing.ticketingId,
                TimeIn = DateTime.Now,
                PlateNumber = ticketing.plateNumber
            };

            ShortTrip shortTrip = new ShortTrip
            {
                ticketingId = ticketing.ticketingId,
                TimeIn = DateTime.Now,
                PlateNumber = ticketing.plateNumber
            };

            if (objGuid == Guid.Empty)
            {
                ticketing.ticketingId = Guid.NewGuid();
                tradersTruck.ticketingId = ticketing.ticketingId;
                farmersTruck.ticketingId = ticketing.ticketingId;
                shortTrip.ticketingId = ticketing.ticketingId;

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    _context.TradersTruck.Add(tradersTruck);
                }
                else if (ticketing.typeOfTransaction == "Farmer truck")
                {
                    _context.FarmersTruck.Add(farmersTruck);
                }
                else
                {
                    _context.ShortTrip.Add(shortTrip);
                }

                if (ticketing.typeOfTransaction == "Trader truck" && ticketing.typeOfCar == "Single tire")
                {
                    ticketing.amount = 50;
                }
                    _context.Ticketing.Add(ticketing);
            }
            else
            {
                ticketing.ticketingId = objGuid;
                tradersTruck.ticketingId = ticketing.ticketingId;
                farmersTruck.ticketingId = ticketing.ticketingId;
                shortTrip.ticketingId = ticketing.ticketingId;

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    _context.TradersTruck.Update(tradersTruck);
                }
                else if (ticketing.typeOfTransaction == "Farmer truck")
                {
                    _context.FarmersTruck.Update(farmersTruck);
                }
                else
                {
                    _context.ShortTrip.Update(shortTrip);
                }

                _context.Ticketing.Update(ticketing);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/UpdateTicketOut
        [HttpPost("UpdateTicketOut")]
        public async Task<IActionResult> UpdateTicketOut(Guid id)
        {
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            ticketing.timeOut = DateTime.Now;
            _context.Ticketing.Update(ticketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/PostGatePass
        [HttpPost("PostGatePass")]
        public async Task<IActionResult> PostGatePass([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            GatePass gatePass = new GatePass
            {
                BirthDate = Convert.ToDateTime(model["BirthDate"].ToString()),
                FirstName = model["FirstName"].ToString(),
                LastName = model["LastName"].ToString(),
                PlateNumber1 = model["PlateNumber1"].ToString(),
                PlateNumber2 = model["PlateNumber2"].ToString(),
                Status = Convert.ToInt32(model["Status"].ToString()),
                StartDate = Convert.ToDateTime(model["StartDate"].ToString()),
                EndDate = Convert.ToDateTime(model["EndDate"].ToString()),
                ContactNumber = model["ContactNumber"].ToString(),
                IdType = model["IdType"].ToString(),
                IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Remarks = model["Remarks"].ToString()
            };
            if (id == 0)
            {
                //gatePass.Id = id();
                _context.GatePass.Add(gatePass);
            }
            else
            {
                gatePass.Id = id;
                _context.GatePass.Update(gatePass);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Ticketing/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketing([FromRoute] Guid id)
        {
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(ticketing);
            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(tradersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Ticketing/DeleteGatePass
        [HttpDelete("DeleteGatePass/{id}")]
        public async Task<IActionResult> DeleteGatePass([FromRoute] int id)
        {
            GatePass gatePass = _context.GatePass.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(gatePass);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}