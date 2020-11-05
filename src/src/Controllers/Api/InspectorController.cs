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

            if (tradersTruck.EstimatedVolume == null)
            {
            }
            else if (tradersTruck.EstimatedVolume != null)
            {
                var one = "";
                var two = "";
                var three = "";
                var four = "";
                var five = "";
                if (tradersTruck.PlateNumber != model["PlateNumber"].ToString() || tradersTruck.TraderName != model["TraderName"].ToString() || tradersTruck.Destination != model["Destination"].ToString() || tradersTruck.EstimatedVolume != Convert.ToInt32(model["EstimatedVolume"].ToString()) || tradersTruck.Remarks != model["Remarks"].ToString())
                {
                    if (tradersTruck.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "Plate number = " + tradersTruck.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (tradersTruck.TraderName != model["TraderName"].ToString())
                    {
                        two = " Trader name = " + tradersTruck.TraderName + " - " + model["TraderName"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (tradersTruck.Destination != model["Destination"].ToString())
                    {
                        three = " Destination = " + tradersTruck.Destination + " - " + model["Destination"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (tradersTruck.EstimatedVolume != Convert.ToInt32(model["EstimatedVolume"].ToString()))
                    {
                        four = " Estimated volume = " + tradersTruck.EstimatedVolume + " - " + model["EstimatedVolume"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (tradersTruck.Remarks != model["Remarks"].ToString())
                    {
                        five = " Remarks = " + tradersTruck.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    var datas = one + two + three + four + five;

                EditedDatas editedDatas = new EditedDatas
                {
                    DateEdited = DateTime.Now,
                    Origin = "Traders truck",
                    EditedBy = info.FullName,
                    ControlNumber = tradersTruck.ControlNumber.Value
                };
                editedDatas.EditedData = datas;
                editedDatas.Remarks = model["Remarks"].ToString();
                _context.EditedDatas.Add(editedDatas);
                }
            }

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

            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            ticketing.plateNumber = tradersTruck.PlateNumber;
            _context.Ticketing.Update(ticketing);

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
            Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();

            if (farmersTruck.Commodity == "")
            {
            }
            else if (farmersTruck.Commodity != null)
            {
                var one = "";
                var two = "";
                var three = "";
                var four = "";
                var five = "";
                var six = "";
                var seven = "";
                var eight = "";
                var nine = "";
                if (farmersTruck.PlateNumber != model["PlateNumber"].ToString() || farmersTruck.Barangay != model["Barangay"].ToString() || farmersTruck.FarmersName != model["FarmersName"].ToString() || farmersTruck.Organization != model["Organization"].ToString() || farmersTruck.Commodity != model["Commodity"].ToString() || farmersTruck.Volume != Convert.ToInt32(model["Volume"].ToString()) || farmersTruck.StallNumber != model["StallNumber"].ToString() || farmersTruck.FacilitatorsName != model["FacilitatorsName"].ToString() || farmersTruck.Remarks != model["Remarks"].ToString())
                {
                    if (farmersTruck.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "Plate number = " + farmersTruck.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (farmersTruck.Barangay != model["Barangay"].ToString())
                    {
                        if (addresses != null)
                        {
                            two = " Barangay = " + farmersTruck.Barangay + " - " + model["Barangay"].ToString() + ";";
                        }
                        if (addresses == null)
                        {
                            two = " Barangay = " + farmersTruck.Barangay + " - " + " ;";
                        }
                    }
                    else
                    {
                        two = "";
                    }
                    if (farmersTruck.FarmersName != model["FarmersName"].ToString())
                    {
                        three = " Farmers name = " + farmersTruck.FarmersName + " - " + model["FarmersName"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (farmersTruck.Organization != model["Organization"].ToString())
                    {
                        four = " Organization = " + farmersTruck.Organization + " - " + model["Organization"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (farmersTruck.Commodity != model["Commodity"].ToString())
                    {
                        five = " Commodity = " + farmersTruck.Commodity + " - " + model["Commodity"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (farmersTruck.Volume != Convert.ToInt32(model["Volume"].ToString()))
                    {
                        six = " Volume = " + farmersTruck.Volume + " - " + model["Volume"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    if (farmersTruck.StallNumber != model["StallNumber"].ToString())
                    {
                        seven = " Stall number = " + farmersTruck.StallNumber + " - " + model["StallNumber"].ToString() + ";";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (farmersTruck.FacilitatorsName != model["FacilitatorsName"].ToString())
                    {
                        eight = " Facilitators name = " + farmersTruck.FacilitatorsName + " - " + model["FacilitatorsName"].ToString() + ";";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (farmersTruck.Remarks != model["Remarks"].ToString())
                    {
                        nine = " Remarks = " + farmersTruck.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        nine = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine;

                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Farmers truck",
                        EditedBy = info.FullName,
                        ControlNumber = farmersTruck.ControlNumber.Value
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
            }

            farmersTruck.DateInspected = DateTime.Now;
            farmersTruck.StallNumber = model["StallNumber"].ToString();
            farmersTruck.PlateNumber = model["PlateNumber"].ToString();
            farmersTruck.FarmersName = model["FarmersName"].ToString();
            farmersTruck.Organization = model["Organization"].ToString();
            farmersTruck.FacilitatorsName = model["FacilitatorsName"].ToString();
            farmersTruck.Remarks = model["Remarks"].ToString();

            if (addresses != null)
            {
                farmersTruck.Barangay = addresses.Barangay;
                farmersTruck.Municipality = addresses.Municipality;
                farmersTruck.Province = addresses.Province;
            }
            else if (addresses == null)
            {
                farmersTruck.Barangay = "";
                farmersTruck.Municipality = "";
                farmersTruck.Province = "";
            }
            //else
            //{
            //    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
            //}

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

            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            ticketing.plateNumber = farmersTruck.PlateNumber;
            _context.Ticketing.Update(ticketing);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostShortTrip
        [HttpPost("PostShortTrip")]
        public async Task<IActionResult> PostShortTrip([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            Guid ticketingId = Guid.Empty;
            ticketingId = Guid.Parse(model["ticketingId"].ToString());
            var info = await _userManager.GetUserAsync(User);

            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.Id == id).FirstOrDefault();

            if (shortTrip.CommodityIn == "")
            {
            }
            else if (shortTrip.CommodityIn != null)
            {
                var one = "";
                var two = "";
                var three = "";
                var four = "";
                if (shortTrip.PlateNumber != model["PlateNumber"].ToString() || shortTrip.CommodityIn != model["CommodityIn"].ToString() || shortTrip.EstimatedVolumeIn != Convert.ToInt32(model["EstimatedVolumeIn"].ToString()) || shortTrip.RemarksIn != model["RemarksIn"].ToString())
                {
                    if (shortTrip.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "Plate number = " + shortTrip.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (shortTrip.CommodityIn != model["CommodityIn"].ToString())
                    {
                        two = " Commodity(in) = " + shortTrip.CommodityIn + " - " + model["CommodityIn"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (shortTrip.EstimatedVolumeIn != Convert.ToInt32(model["EstimatedVolumeIn"].ToString()))
                    {
                        three = " Estimated volume(in) = " + shortTrip.EstimatedVolumeIn + " - " + model["EstimatedVolumeIn"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (shortTrip.RemarksIn != model["RemarksIn"].ToString())
                    {
                        four = " Remarks(in) = " + shortTrip.RemarksIn + " - " + model["RemarksIn"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    EditedDatas editedDatas = new EditedDatas
                {
                    DateEdited = DateTime.Now,
                    Origin = "Short trip/Edit in",
                    EditedBy = info.FullName,
                    ControlNumber = shortTrip.ControlNumber.Value
                };
                    var datas = one + two + three + four;
                    editedDatas.EditedData = datas;
                editedDatas.Remarks = model["RemarksIn"].ToString();
                _context.EditedDatas.Add(editedDatas);
                }
            }

            shortTrip.DateInspectedIn = DateTime.Now;
            shortTrip.RemarksIn = model["RemarksIn"].ToString();
            shortTrip.PlateNumber = model["PlateNumber"].ToString();

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

            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == ticketingId).FirstOrDefault();
            ticketing.plateNumber = shortTrip.PlateNumber;
            _context.Ticketing.Update(ticketing);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Inspector/PostShortTripOut
        [HttpPost("PostShortTripOut")]
        public async Task<IActionResult> PostShortTripOut([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            Guid ticketingId = Guid.Empty;
            ticketingId = Guid.Parse(model["ticketingId"].ToString());
            var info = await _userManager.GetUserAsync(User);

            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.Id == id).FirstOrDefault();

            if (shortTrip.CommodityOut == "")
            {
            }
            else if (shortTrip.CommodityOut != null)
            {
                var one = "";
                var two = "";
                var three = "";
                var four = "";
                if (shortTrip.PlateNumber != model["PlateNumber"].ToString() || shortTrip.CommodityOut != model["CommodityOut"].ToString() || shortTrip.EstimatedVolumeOut != Convert.ToInt32(model["EstimatedVolumeOut"].ToString()) || shortTrip.RemarksOut != model["RemarksOut"].ToString())
                {
                    if (shortTrip.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "Plate number = " + shortTrip.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (shortTrip.CommodityOut != model["CommodityOut"].ToString())
                    {
                        two = " Commodity(out) = " + shortTrip.CommodityOut + " - " + model["CommodityOut"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (shortTrip.EstimatedVolumeOut != Convert.ToInt32(model["EstimatedVolumeOut"].ToString()))
                    {
                        three = " Estimated volume(out) = " + shortTrip.EstimatedVolumeOut + " - " + model["EstimatedVolumeOut"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (shortTrip.RemarksOut != model["RemarksOut"].ToString())
                    {
                        four = " Remarks(out) = " + shortTrip.RemarksOut + " - " + model["RemarksOut"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Short trip/Edit out",
                        EditedBy = info.FullName,
                        ControlNumber = shortTrip.ControlNumber.Value
                    };
                    var datas = one + two + three + four;
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["RemarksOut"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
            }

            shortTrip.DateInspectedOut = DateTime.Now;
            shortTrip.RemarksOut = model["RemarksOut"].ToString();
            shortTrip.PlateNumber = model["PlateNumber"].ToString();

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

            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == ticketingId).FirstOrDefault();
            ticketing.plateNumber = shortTrip.PlateNumber;
            _context.Ticketing.Update(ticketing);

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
            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["Commodity"].ToString()).FirstOrDefault();

            if (id == 0)
            {
                InterTrading interTrading = new InterTrading
                {
                    Date = DateTime.Now,
                    FarmerName = model["FarmerName"].ToString(),
                    FarmersOrganization = model["FarmersOrganization"].ToString(),
                    Volume = Convert.ToInt32(model["Volume"].ToString()),
                    ProductionArea = model["ProductionArea"].ToString(),
                    Remarks = model["Remarks"].ToString()
                };

                if (commodities != null)
                {
                    interTrading.Commodity = commodities.Commodity;
                }
                else
                {
                    return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
                }
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
                var currentInterTrading = _context.InterTrading.Where(x => x.Id == id).FirstOrDefault();
                if (currentInterTrading.FarmerName != model["FarmerName"].ToString() || currentInterTrading.FarmersOrganization != model["FarmersOrganization"].ToString() || currentInterTrading.Commodity != model["Commodity"].ToString() || currentInterTrading.Volume != Convert.ToInt32(model["Volume"].ToString()) || currentInterTrading.ProductionArea != model["ProductionArea"].ToString() || currentInterTrading.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    var four = "";
                    var five = "";
                    var six = "";
                    if (currentInterTrading.FarmerName != model["FarmerName"].ToString())
                    {
                        one = "Farmers name = " + currentInterTrading.FarmerName + " - " + model["FarmerName"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentInterTrading.FarmersOrganization != model["FarmersOrganization"].ToString())
                    {
                        two = " Farmers organization = " + currentInterTrading.FarmersOrganization + " - " + model["FarmersOrganization"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentInterTrading.Commodity != model["Commodity"].ToString())
                    {
                        three = " Commodity = " + currentInterTrading.Commodity + " - " + model["Commodity"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentInterTrading.Volume != Convert.ToInt32(model["Volume"].ToString()))
                    {
                        four = " Volume = " + currentInterTrading.Volume + " - " + model["Volume"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentInterTrading.ProductionArea != model["ProductionArea"].ToString())
                    {
                        five = " Production area = " + currentInterTrading.ProductionArea + " - " + model["ProductionArea"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentInterTrading.Remarks != model["Remarks"].ToString())
                    {
                        six = " Remarks = " + currentInterTrading.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    var datas = one + two + three + four + five + six;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Inter trading",
                        EditedBy = info.FullName,
                        ControlNumber = currentInterTrading.Code.Value
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                currentInterTrading.FarmerName = model["FarmerName"].ToString();
                currentInterTrading.FarmersOrganization = model["FarmersOrganization"].ToString();
                currentInterTrading.Volume = Convert.ToInt32(model["Volume"].ToString());
                currentInterTrading.ProductionArea = model["ProductionArea"].ToString();
                currentInterTrading.Remarks = model["Remarks"].ToString();
                if (commodities != null)
                {
                    currentInterTrading.Commodity = commodities.Commodity;
                }
                else
                {
                    return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
                }
                currentInterTrading.Inspector = info.FullName;
                _context.InterTrading.Update(currentInterTrading);
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
            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["Commodity"].ToString()).FirstOrDefault();
            MarketFacilitators marketFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.Name == model["Facilitator"].ToString()).FirstOrDefault();

            if (id == 0)
            {
                CarrotFacility carrotFacility = new CarrotFacility
                {
                    Date = DateTime.Now,
                    StallNumber = model["StallNumber"].ToString(),
                    Facilitator = model["Facilitator"].ToString(),
                    Volume = Convert.ToInt32(model["Volume"].ToString()),
                    Destination = model["Destination"].ToString(),
                    Remarks = model["Remarks"].ToString()
                };

                if (commodities != null)
                {
                    carrotFacility.Commodity = commodities.Commodity;
                }
                else
                {
                    return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
                }

                if (marketFacilitators != null)
                {
                    carrotFacility.AccreditationChecker = "Accredited";
                }
                else
                {
                    carrotFacility.AccreditationChecker = "Not accredited";
                }
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
                var currentCarrot = _context.CarrotFacility.Where(x => x.Id == id).FirstOrDefault();
                if (currentCarrot.StallNumber != model["StallNumber"].ToString() || currentCarrot.Facilitator != model["Facilitator"].ToString() || currentCarrot.Commodity != model["Commodity"].ToString() || currentCarrot.Volume != Convert.ToInt32(model["Volume"].ToString()) || currentCarrot.Destination != model["Destination"].ToString() || currentCarrot.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    var four = "";
                    var five = "";
                    var six = "";
                    if (currentCarrot.StallNumber != model["StallNumber"].ToString())
                    {
                        one = "Stall number = " + currentCarrot.StallNumber + " - " + model["StallNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentCarrot.Facilitator != model["Facilitator"].ToString())
                    {
                        two = " Facilitator = " + currentCarrot.Facilitator + " - " + model["Facilitator"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentCarrot.Commodity != model["Commodity"].ToString())
                    {
                        three = " Commodity = " + currentCarrot.Commodity + " - " + model["Commodity"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentCarrot.Volume != Convert.ToInt32(model["Volume"].ToString()))
                    {
                        four = " Volume = " + currentCarrot.Volume + " - " + model["Volume"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentCarrot.Destination != model["Destination"].ToString())
                    {
                        five = " Destination = " + currentCarrot.Destination + " - " + model["Destination"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentCarrot.Remarks != model["Remarks"].ToString())
                    {
                        six = " Remarks = " + currentCarrot.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    var datas = one + two + three + four + five + six;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Carrot facility",
                        EditedBy = info.FullName,
                        ControlNumber = currentCarrot.Code.Value
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                currentCarrot.StallNumber = model["StallNumber"].ToString();
                currentCarrot.Facilitator = model["Facilitator"].ToString();
                currentCarrot.Commodity = model["Commodity"].ToString();
                currentCarrot.Volume = Convert.ToInt32(model["Volume"].ToString());
                currentCarrot.Destination = model["Destination"].ToString();
                currentCarrot.Remarks = model["Remarks"].ToString();
                currentCarrot.Inspector = info.FullName;
                _context.CarrotFacility.Update(currentCarrot);
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