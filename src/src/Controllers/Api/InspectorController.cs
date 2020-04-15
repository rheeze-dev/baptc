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
        private readonly SignInManager<ApplicationUser> _signInManager;


        public InspectorController(ApplicationDbContext context,
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

        // GET: api/Inspector/GetTradersTruck
        [HttpGet("GetTradersTruck")]
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

        // GET: api/Inspector/GetPayParking
        [HttpGet("GetPayParking")]
        public IActionResult GetPayParking([FromRoute]Guid organizationId)
        {
            var payParking = _context.PayParking.ToList();
            return Json(new { data = payParking });
        }

        // GET: api/Inspector/GetInterTrading
        [HttpGet("GetInterTrading")]
        public IActionResult GetInterTrading([FromRoute]Guid organizationId)
        {
            var interTrading = _context.InterTrading.ToList();
            return Json(new { data = interTrading });
        }

        // GET: api/Inspector/GetCarrotFacility
        [HttpGet("GetCarrotFacility")]
        public IActionResult GetCarrotFacility([FromRoute]Guid organizationId)
        {
            var carrotFacility = _context.CarrotFacility.ToList();
            return Json(new { data = carrotFacility });
        }

        // POST: api/Inspector
        [HttpPost]
        public async Task<IActionResult> PostTradersTruck([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            id = Guid.Parse(model["ticketingId"].ToString());
            var info = await _userManager.GetUserAsync(User);

            TradersTruck tradersTruck = new TradersTruck
            {
                DateInspected = DateTime.Now,
                TimeIn = Convert.ToDateTime(model["TimeIn"].ToString()),
                TraderName = model["TraderName"].ToString(),
                PlateNumber = model["PlateNumber"].ToString(),
                Destination = model["Destination"].ToString()
                //EstimatedVolume = Convert.ToInt32(model["EstimatedVolume"].ToString())
            };

            if (model["EstimatedVolume"].ToString() == "")
            {
                return Json(new { success = false, message = "Estimated volume cannot be empty!" });
            }
            tradersTruck.EstimatedVolume = Convert.ToInt32(model["EstimatedVolume"].ToString());

            tradersTruck.ticketingId = id;
            tradersTruck.Inspector = info.FullName;
            _context.TradersTruck.Update(tradersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostFarmersTruck
        [HttpPost("PostFarmersTruck")]
        public async Task<IActionResult> PostFarmersTruck([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            id = Guid.Parse(model["ticketingId"].ToString());
            var info = await _userManager.GetUserAsync(User);

            FarmersTruck farmersTruck = new FarmersTruck
            {
                DateInspected = DateTime.Now,
                TimeIn = Convert.ToDateTime(model["TimeIn"].ToString()),
                StallNumber = model["StallNumber"].ToString(),
                PlateNumber = model["PlateNumber"].ToString(),
                FarmersName = model["FarmersName"].ToString(),
                Organization = model["Organization"].ToString(),
                Commodity = model["Commodity"].ToString(),
                Barangay = model["Barangay"].ToString(),
                Province = model["Province"].ToString(),
                FacilitatorsName = model["FacilitatorsName"].ToString()
            };

            if (model["Volume"].ToString() == "")
            {
                return Json(new { success = false, message = "Volume cannot be empty!" });
            }

            farmersTruck.Volume = Convert.ToInt32(model["Volume"].ToString());

            farmersTruck.ticketingId = id;
            farmersTruck.Inspector = info.FullName;
            _context.FarmersTruck.Update(farmersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostShortTrip
        [HttpPost("PostShortTrip")]
        public async Task<IActionResult> PostShortTrip([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            id = Guid.Parse(model["ticketingId"].ToString());
            var info = await _userManager.GetUserAsync(User);

            ShortTrip shortTrip = new ShortTrip
            {
                DateInspected = DateTime.Now,
                TimeIn = Convert.ToDateTime(model["TimeIn"].ToString()),
                PlateNumber = model["PlateNumber"].ToString(),
                Commodity = model["Commodity"].ToString(),
            };

            if (model["EstimatedVolume"].ToString() == "")
            {
                return Json(new { success = false, message = "Volume cannot be empty!" });
            }

            shortTrip.EstimatedVolume = Convert.ToInt32(model["EstimatedVolume"].ToString());

            shortTrip.ticketingId = id;
            shortTrip.Inspector = info.FullName;
            _context.ShortTrip.Update(shortTrip);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostPayParking
        [HttpPost("PostPayParking")]
        public async Task<IActionResult> PostPayParking([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            id = Guid.Parse(model["ticketingId"].ToString());

            PayParking payParking = new PayParking
            {
                DateInspected = DateTime.Now,
                TimeIn = Convert.ToDateTime(model["TimeIn"].ToString()),
                PlateNumber = model["PlateNumber"].ToString(),
                DriverName = model["DriverName"].ToString()
            };

            payParking.ticketingId = id;
            _context.PayParking.Update(payParking);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostInterTrading
        [HttpPost("PostInterTrading")]
        public async Task<IActionResult> PostInterTrading([FromBody] JObject model)
        {
            int id = 0;
            var getLastCode = _context.InterTrading.OrderByDescending(x => x.Code).Select(x => x.Code).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());

            InterTrading interTrading = new InterTrading
            {
                Date = DateTime.Now,
                FarmerName = model["FarmerName"].ToString(),
                FarmersOrganization = model["FarmersOrganization"].ToString(),
                Commodity = model["Commodity"].ToString(),
                Volume = Convert.ToInt32(model["Volume"].ToString()),
                //Code = Convert.ToInt32(model["Volume"].ToString()),
                ProductionArea = model["ProductionArea"].ToString()

            };
            
            if (id == 0)
            {
                if (getLastCode == null)
                {
                    interTrading.Code = 1;
                }
                else
                {
                    interTrading.Code = getLastCode + 1;
                }
                interTrading.Inspector = info.FullName;
                _context.InterTrading.Add(interTrading);
            }
            else
            {
                interTrading.Id = id;
                interTrading.Code = Convert.ToInt32(model["Code"].ToString());
                interTrading.Inspector = info.FullName;
                _context.InterTrading.Update(interTrading);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostCarrotFacility
        [HttpPost("PostCarrotFacility")]
        public async Task<IActionResult> PostCarrotFacility([FromBody] JObject model)
        {
            int id = 0;
            var getLastCode = _context.CarrotFacility.OrderByDescending(x => x.Code).Select(x => x.Code).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());

            CarrotFacility carrotFacility = new CarrotFacility
            {
                Date = DateTime.Now,
                StallNumber = model["StallNumber"].ToString(),
                Facilitator = model["Facilitator"].ToString(),
                Commodity = model["Commodity"].ToString(),
                Volume = Convert.ToInt32(model["Volume"].ToString()),
                Destination = model["Destination"].ToString()

            };
            
            if (id == 0)
            {
                if (getLastCode == null)
                {
                    carrotFacility.Code = 1;
                }
                else
                {
                    carrotFacility.Code = getLastCode + 1;
                }
                carrotFacility.Inspector = info.FullName;
                _context.CarrotFacility.Add(carrotFacility);
            }
            else
            {
                carrotFacility.Id = id;
                carrotFacility.Code = Convert.ToInt32(model["Code"].ToString());
                carrotFacility.Inspector = info.FullName;
                _context.CarrotFacility.Update(carrotFacility);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Inspector/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTradersTruck([FromRoute] Guid id)
        {
            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(tradersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteFarmersTruck
        [HttpDelete("Farmers/{id}")]
        public async Task<IActionResult> DeleteFarmersTruck([FromRoute] Guid id)
        {
            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(farmersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteShortTrip
        [HttpDelete("ShortTrip/{id}")]
        public async Task<IActionResult> DeleteShortTrip([FromRoute] Guid id)
        {
            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(shortTrip);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteInterTrading
        [HttpDelete("InterTrading/{id}")]
        public async Task<IActionResult> DeleteInterTrading([FromRoute] int id)
        {
            InterTrading interTrading = _context.InterTrading.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(interTrading);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteCarrotFacility
        [HttpDelete("CarrotFacility/{id}")]
        public async Task<IActionResult> DeleteCarrotFacility([FromRoute] int id)
        {
            CarrotFacility carrotFacility = _context.CarrotFacility.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(carrotFacility);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }


        //// POST: api/Inspector/GetVehicles
        //[HttpGet("GetVehicles")]
        //public IActionResult GetVehicles()
        //{
        //    List<Ticketing> currentVehicles = _context.Ticketing.OrderBy(x => x.timeIn).ToList();
        //    return Json(new { data = currentVehicles });
        //}

    }
}