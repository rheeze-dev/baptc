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
            var tradersTruck = _context.TradersTruck.OrderByDescending(x => x.TimeIn).ToList();
            return Json(new { data = tradersTruck });
        }

        // GET: api/Inspector/GetFarmersTruck
        [HttpGet("GetFarmersTruck")]
        public IActionResult GetFarmersTruck([FromRoute]Guid organizationId)
        {
            var farmersTruck = _context.FarmersTruck.OrderByDescending(x => x.TimeIn).ToList();
            return Json(new { data = farmersTruck });
        }

        // GET: api/Inspector/GetShortTrip
        [HttpGet("GetShortTrip")]
        public IActionResult GetShortTrip([FromRoute]Guid organizationId)
        {
            var shortTrip = _context.ShortTrip.OrderByDescending(x => x.TimeIn).ToList();
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

            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            tradersTruck.DateInspected = DateTime.Now;
            tradersTruck.TraderName = model["TraderName"].ToString();
            tradersTruck.PlateNumber = model["PlateNumber"].ToString();
            tradersTruck.Destination = model["Destination"].ToString();
            tradersTruck.Remarks = model["Remarks"].ToString();
            if (model["EstimatedVolume"].ToString() == "")
            {
                return Json(new { success = false, message = "Estimated volume cannot be empty!" });
            }
            tradersTruck.EstimatedVolume = Convert.ToInt32(model["EstimatedVolume"].ToString());
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

            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            farmersTruck.DateInspected = DateTime.Now;
            farmersTruck.StallNumber = model["StallNumber"].ToString();
            farmersTruck.PlateNumber = model["PlateNumber"].ToString();
            farmersTruck.FarmersName = model["FarmersName"].ToString();
            farmersTruck.Organization = model["Organization"].ToString();
            farmersTruck.FacilitatorsName = model["FacilitatorsName"].ToString();
            farmersTruck.Remarks = model["Remarks"].ToString();

            Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
            if (addresses != null)
            {
                farmersTruck.Barangay = addresses.Barangay;
                farmersTruck.Municipality = addresses.Municipality;
                farmersTruck.Province = addresses.Province;
            }
            else
            {
                return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
            }

            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["Commodity"].ToString()).FirstOrDefault();
            if (commodities != null)
            {
                farmersTruck.Commodity = commodities.Commodity;
            }
            else
            {
                return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
            }

            if (model["Volume"].ToString() == "")
            {
                return Json(new { success = false, message = "Volume cannot be empty!" });
            }
            else if (Convert.ToInt32(model["Volume"].ToString()) == 0)
            {
                return Json(new { success = false, message = "Volume cannot be 0!" });
            }

            farmersTruck.Volume = Convert.ToInt32(model["Volume"].ToString());
            farmersTruck.Inspector = info.FullName;

            MarketFacilitators marketFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.Name == model["FacilitatorsName"].ToString()).FirstOrDefault();
            if (marketFacilitators != null)
            {
                farmersTruck.AccreditationChecker = "Accredited";
            }
            else
            {
                farmersTruck.AccreditationChecker = "Not accredited";
            }

            _context.FarmersTruck.Update(farmersTruck);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostShortTrip
        [HttpPost("PostShortTrip")]
        public async Task<IActionResult> PostShortTrip([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            var info = await _userManager.GetUserAsync(User);

            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.Id == id).FirstOrDefault();
            shortTrip.DateInspectedIn = DateTime.Now;
            shortTrip.RemarksIn = model["RemarksIn"].ToString();

            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["CommodityIn"].ToString()).FirstOrDefault();
            if (commodities != null)
            {
                shortTrip.CommodityIn = commodities.Commodity;
            }
            else
            {
                return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
            }

            if (model["EstimatedVolumeIn"].ToString() == "")
            {
                return Json(new { success = false, message = "Volume cannot be empty!" });
            }

            shortTrip.EstimatedVolumeIn = Convert.ToInt32(model["EstimatedVolumeIn"].ToString());
            shortTrip.InspectorIn = info.FullName;
            _context.ShortTrip.Update(shortTrip);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostShortTripOut
        [HttpPost("PostShortTripOut")]
        public async Task<IActionResult> PostShortTripOut([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            var info = await _userManager.GetUserAsync(User);

            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.Id == id).FirstOrDefault();
            shortTrip.DateInspectedOut = DateTime.Now;
            shortTrip.RemarksOut = model["RemarksOut"].ToString();

            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["CommodityOut"].ToString()).FirstOrDefault();
            if (commodities != null)
            {
                shortTrip.CommodityOut = commodities.Commodity;
            }
            else
            {
                return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
            }

            if (model["EstimatedVolumeOut"].ToString() == "")
            {
                return Json(new { success = false, message = "Volume cannot be empty!" });
            }

            shortTrip.EstimatedVolumeOut = Convert.ToInt32(model["EstimatedVolumeOut"].ToString());
            shortTrip.InspectorOut = info.FullName;
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
                Volume = Convert.ToInt32(model["Volume"].ToString()),
                ProductionArea = model["ProductionArea"].ToString(),
                Remarks = model["Remarks"].ToString()
            };

            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["Commodity"].ToString()).FirstOrDefault();
            if (commodities != null)
            {
                interTrading.Commodity = commodities.Commodity;
            }
            else
            {
                return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
            }

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
                Volume = Convert.ToInt32(model["Volume"].ToString()),
                Destination = model["Destination"].ToString(),
                Remarks = model["Remarks"].ToString()
            };

            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["Commodity"].ToString()).FirstOrDefault();
            if (commodities != null)
            {
                carrotFacility.Commodity = commodities.Commodity;
            }
            else
            {
                return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
            }

            MarketFacilitators marketFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.Name == model["Facilitator"].ToString()).FirstOrDefault();
            if (marketFacilitators != null)
            {
                carrotFacility.AccreditationChecker = "Accredited";
            }
            else
            {
                carrotFacility.AccreditationChecker = "Not accredited";
            }

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

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = tradersTruck.PlateNumber,
                Origin = "Traders truck",
                Name = tradersTruck.TraderName,
                DeletedBy = info.FullName,
                Remarks = tradersTruck.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteFarmersTruck
        [HttpDelete("Farmers/{id}")]
        public async Task<IActionResult> DeleteFarmersTruck([FromRoute] Guid id)
        {
            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(farmersTruck);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = farmersTruck.PlateNumber,
                Origin = "Farmers truck",
                Name = farmersTruck.FarmersName,
                DeletedBy = info.FullName,
                Remarks = farmersTruck.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteShortTrip
        [HttpDelete("ShortTrip/{id}")]
        public async Task<IActionResult> DeleteShortTrip([FromRoute] Guid id)
        {
            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(shortTrip);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = shortTrip.PlateNumber,
                Origin = "Short trip",
                Name = "",
                DeletedBy = info.FullName,
                Remarks = shortTrip.RemarksIn + shortTrip.RemarksOut
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteInterTrading
        [HttpDelete("InterTrading/{id}")]
        public async Task<IActionResult> DeleteInterTrading([FromRoute] int id)
        {
            InterTrading interTrading = _context.InterTrading.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(interTrading);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Inter-trading",
                Name = interTrading.FarmerName,
                DeletedBy = info.FullName,
                Remarks = interTrading.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Inspector/DeleteCarrotFacility
        [HttpDelete("CarrotFacility/{id}")]
        public async Task<IActionResult> DeleteCarrotFacility([FromRoute] int id)
        {
            CarrotFacility carrotFacility = _context.CarrotFacility.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(carrotFacility);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Carrot facility",
                Name = "",
                DeletedBy = info.FullName,
                Remarks = carrotFacility.Remarks
            };

            _context.DeletedDatas.Add(deleted);

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