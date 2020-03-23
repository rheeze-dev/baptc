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
                driverName = model["driverName"].ToString(),
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

            PayParking payParking = new PayParking
            {
                ticketingId = ticketing.ticketingId,
                TimeIn = DateTime.Now,
                PlateNumber = ticketing.plateNumber,
                DriverName = ticketing.driverName
            };

            if (objGuid == Guid.Empty)
            {
                ticketing.ticketingId = Guid.NewGuid();
                tradersTruck.ticketingId = ticketing.ticketingId;
                farmersTruck.ticketingId = ticketing.ticketingId;
                shortTrip.ticketingId = ticketing.ticketingId;
                payParking.ticketingId = ticketing.ticketingId;

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    _context.TradersTruck.Add(tradersTruck);
                }
                else if (ticketing.typeOfTransaction == "Farmer truck")
                {
                    _context.FarmersTruck.Add(farmersTruck);
                }
                else if (ticketing.typeOfTransaction == "Short trip")
                {
                    _context.ShortTrip.Add(shortTrip);
                }
                else
                {
                    _context.PayParking.Add(payParking);
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
                payParking.ticketingId = ticketing.ticketingId;

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    _context.TradersTruck.Update(tradersTruck);
                }
                else if (ticketing.typeOfTransaction == "Farmer truck")
                {
                    _context.FarmersTruck.Update(farmersTruck);
                }
                else if (ticketing.typeOfTransaction == "Short trip")
                {
                    _context.ShortTrip.Update(shortTrip);
                }
                else
                {
                    _context.PayParking.Update(payParking);
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

        // POST: api/Ticketing/ExtendGatePass
        [HttpPost("ExtendGatePass")]
        public async Task<IActionResult> ExtendGatePass(Guid id)
        {
            //Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == id).FirstOrDefault();
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();

            //ticketing.timeOut = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            DateTime endDate = new DateTime(currentDate.Year, 12, 31);

            gatePass.EndDate = endDate;
            ticketing.endDate = gatePass.EndDate;
            //gatePass.EndDate = gatePass.EndDate.Value.AddYears(1);
            gatePass.Amount = gatePass.Amount + 500;
            ticketing.amount = ticketing.amount + 500;


            //_context.Ticketing.Update(ticketing);
            _context.GatePass.Update(gatePass);
            _context.Ticketing.Update(ticketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/PostGatePass
        [HttpPost("PostGatePass")]
        public async Task<IActionResult> PostGatePass([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            DateTime startDate = DateTime.Now;
            //DateTime endDate = startDate.AddYears(1);
            var endDate = new DateTime(startDate.Year, 12, 31);
            int amount = 500;
            id = Guid.Parse(model["ticketingId"].ToString());
            //id = Convert.ToInt32(model["Id"].ToString());
            GatePass gatePass = new GatePass
            {
                StartDate = startDate,
                EndDate = endDate,
                //BirthDate = Convert.ToDateTime(model["BirthDate"].ToString()),
                DriverName = model["DriverName"].ToString(),
                //LastName = model["LastName"].ToString(),
                PlateNumber1 = model["PlateNumber1"].ToString(),
                PlateNumber2 = model["PlateNumber2"].ToString(),
                //Status = Convert.ToInt32(model["Status"].ToString()),
                //ContactNumber = model["ContactNumber"].ToString(),
                //IdType = model["IdType"].ToString(),
                //IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Remarks = model["Remarks"].ToString(),
                Amount = amount
                
            };

            Ticketing ticketing = new Ticketing
            {
                ticketingId = gatePass.ticketingId,
                amount = gatePass.Amount,
                plateNumber = gatePass.PlateNumber1 + ", " + gatePass.PlateNumber2,
                endDate = gatePass.EndDate,
                driverName = gatePass.DriverName
            };
            if (id == Guid.Empty)
            {
                gatePass.ticketingId = Guid.NewGuid();
                ticketing.ticketingId = gatePass.ticketingId;

                //gatePass.Id = id();
                _context.GatePass.Add(gatePass);
                _context.Ticketing.Add(ticketing);
            }
            else
            {
                gatePass.ticketingId = Guid.NewGuid();
                ticketing.ticketingId = gatePass.ticketingId;
                _context.GatePass.Add(gatePass);
                _context.Ticketing.Add(ticketing);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/PostNewGatePass
        [HttpPost("PostNewGatePass")]
        public async Task<IActionResult> PostNewGatePass([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            //DateTime startDate = DateTime.Now;
            //DateTime endDate = startDate.AddYears(1);
            //var endDate = new DateTime(startDate.Year, 12, 31);
            int amount = 0;
            id = Guid.Parse(model["ticketingId"].ToString());
            //id = Convert.ToInt32(model["Id"].ToString());

            Ticketing ticketing = new Ticketing
            {
                timeIn = DateTime.Now,
                //ticketingId = gatePass.ticketingId,
                amount = amount,
                plateNumber = model["plateNumber"].ToString(),
                driverName = model["driverName"].ToString(),
                remarks = model["remarks"].ToString()
            };

            GatePass gatePass = new GatePass
            {
                ticketingId = ticketing.ticketingId,
                //StartDate = startDate,
                //EndDate = endDate,
                //BirthDate = Convert.ToDateTime(model["BirthDate"].ToString()),
                DriverName = ticketing.driverName,
                //LastName = model["LastName"].ToString(),
                PlateNumber1 = ticketing.plateNumber,
                PlateNumber2 = ticketing.plateNumber,
                //Status = Convert.ToInt32(model["Status"].ToString()),
                //ContactNumber = model["ContactNumber"].ToString(),
                //IdType = model["IdType"].ToString(),
                //IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Remarks = ticketing.remarks,
                Amount = amount

            };

            //Ticketing ticketing = new Ticketing
            //{
            //    ticketingId = gatePass.ticketingId,
            //    amount = gatePass.Amount,
            //    plateNumber = gatePass.PlateNumber1 + ", " + gatePass.PlateNumber2,
            //    endDate = gatePass.EndDate,
            //    driverName = gatePass.DriverName
            //};
            if (id == Guid.Empty)
            {
                ticketing.ticketingId = Guid.NewGuid();
                gatePass.ticketingId = ticketing.ticketingId;

                //gatePass.Id = id();
                _context.Ticketing.Add(ticketing);
                _context.GatePass.Add(gatePass);
            }
            else
            {
                ticketing.ticketingId = Guid.NewGuid();
                gatePass.ticketingId = ticketing.ticketingId;
                _context.Ticketing.Add(ticketing);
                _context.GatePass.Add(gatePass);
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
            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(farmersTruck);
            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(shortTrip);
            PayParking payParking = _context.PayParking.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(payParking);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Ticketing/DeleteGatePass
        [HttpDelete("DeleteGatePass/{id}")]
        public async Task<IActionResult> DeleteGatePass([FromRoute] Guid id)
        {
            GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(gatePass);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}