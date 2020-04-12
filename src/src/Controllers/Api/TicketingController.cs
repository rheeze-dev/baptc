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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TicketingController(ApplicationDbContext context,
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

        // GET: api/Ticketing
        [HttpGet("{organizationId}")]
        public IActionResult GetTicketing([FromRoute]Guid organizationId)
        {
            var ticketing = _context.Ticketing.OrderBy(x => x.timeIn).ToList();
            return Json(new { data = ticketing });
        }
    
        // GET: api/Ticketing/GetGatePass
        [HttpGet("GetGatePass")]
        public IActionResult GetGatePass([FromRoute]Guid organizationId)
        {
            var stallLease = _context.StallLease.ToList();
            return Json(new { data = stallLease });
        }

        // POST: api/Ticketing
        [HttpPost]
        public async Task<IActionResult> PostTicketing([FromBody] JObject model)
        {
            Guid objGuid = Guid.Empty;
            var getLastControlNumber = _context.Ticketing.OrderByDescending(x => x.controlNumber).Select(x => x.controlNumber).FirstOrDefault();
            //int? controlNumber = getLastControlNumber + 1;
            var info = await _userManager.GetUserAsync(User);
            objGuid = Guid.Parse(model["ticketingId"].ToString());

            if (objGuid == Guid.Empty)
            {
                Ticketing ticketing = new Ticketing
                {
                    timeIn = DateTime.Now,
                    plateNumber = model["plateNumber"].ToString(),
                    typeOfTransaction = model["typeOfTransaction"].ToString(),
                    typeOfCar = model["typeOfCar"].ToString(),
                    driverName = model["driverName"].ToString(),
                    remarks = model["remarks"].ToString(),
                    amount = null
                };

                if (getLastControlNumber == null)
                {
                    ticketing.controlNumber = 1;
                }
                else
                {
                    ticketing.controlNumber = getLastControlNumber + 1;
                }

                ticketing.ticketingId = Guid.NewGuid();
                ticketing.issuingClerk = info.FullName;

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    if (ticketing.typeOfCar == "With transaction" || ticketing.typeOfCar == "Without transaction" || ticketing.typeOfCar == "Overnight")
                    {
                        _context.Ticketing.Add(ticketing);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                    }
                }

                else if (ticketing.typeOfTransaction == "Farmer truck")
                {
                    if (ticketing.typeOfCar == "Single tire" || ticketing.typeOfCar == "Double tire" || ticketing.typeOfCar == "Forward tire")
                    {
                        _context.Ticketing.Add(ticketing);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                    }
                }

                else if (ticketing.typeOfTransaction == "Short trip")
                {
                    if (ticketing.typeOfCar == "Pick-up" || ticketing.typeOfCar == "Delivery")
                    {
                        _context.Ticketing.Add(ticketing);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                    }
                }

                else if (ticketing.typeOfTransaction == "Pay parking")
                {
                    if (ticketing.typeOfCar == "Day time" || ticketing.typeOfCar == "Night time")
                    {
                        _context.Ticketing.Add(ticketing);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                    }
                    if (ticketing.driverName == "")
                    {
                        return Json(new { success = false, message = "Drivers name cannot be empty!" });
                    }
                }
                else if (ticketing.typeOfTransaction == "Gate pass")
                {
                    if (ticketing.driverName == "")
                    {
                        return Json(new { success = false, message = "Drivers name cannot be empty!" });
                    }

                    ticketing.typeOfCar = null;
                    _context.Ticketing.Add(ticketing);
                }

                CurrentTicket currentTicket = new CurrentTicket
                {
                    ticketingId = ticketing.ticketingId,
                    plateNumber = ticketing.plateNumber,
                    typeOfTransaction = ticketing.typeOfTransaction
                };
                _context.CurrentTicket.Add(currentTicket);

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    TradersTruck tradersTruck = new TradersTruck
                    {
                        ticketingId = ticketing.ticketingId,
                        TimeIn = DateTime.Now,
                        PlateNumber = ticketing.plateNumber,
                        TraderName = "",
                        Destination = ""
                    };

                    _context.TradersTruck.Add(tradersTruck);
                }
                else if (ticketing.typeOfTransaction == "Farmer truck")
                {
                    FarmersTruck farmersTruck = new FarmersTruck
                    {
                        ticketingId = ticketing.ticketingId,
                        TimeIn = DateTime.Now,
                        PlateNumber = ticketing.plateNumber,
                        StallNumber = "",
                        FarmersName = "",
                        Organization = "",
                        Commodity = "",
                        Barangay = ""
                    };

                    _context.FarmersTruck.Add(farmersTruck);
                }
                else if (ticketing.typeOfTransaction == "Short trip")
                {
                    ShortTrip shortTrip = new ShortTrip
                    {
                        ticketingId = ticketing.ticketingId,
                        TimeIn = DateTime.Now,
                        PlateNumber = ticketing.plateNumber,
                        Commodity = ""
                    };

                    _context.ShortTrip.Add(shortTrip);
                }
                else if (ticketing.typeOfTransaction == "Gate pass")
                {
                    //what if?
                    StallLease stallLease = _context.StallLease.Where(x => x.PlateNumber1 == ticketing.plateNumber || x.PlateNumber2 == ticketing.plateNumber).FirstOrDefault();
                    if (stallLease != null)
                    {
                        GatePass gatePass = new GatePass
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            DriverName = ticketing.driverName
                        };
                        if (stallLease.EndDate > DateTime.Now) { 
                            _context.GatePass.Add(gatePass);
                        }
                        else { 
                            return Json(new { success = false, message = "Your gate pass is expired!" });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Vehicle not registered!" });
                    }
                }
                else
                {
                    PayParking payParking = new PayParking
                    {
                        ticketingId = ticketing.ticketingId,
                        TimeIn = DateTime.Now,
                        PlateNumber = ticketing.plateNumber,
                        DriverName = ticketing.driverName
                    };

                    _context.PayParking.Add(payParking);
                }
            }
            else
            {
                Ticketing ticketing = new Ticketing
                {
                    timeIn = DateTime.Now,
                    plateNumber = model["plateNumber"].ToString(),
                    typeOfTransaction = model["typeOfTransaction"].ToString(),
                    typeOfCar = model["typeOfCar"].ToString(),
                    driverName = model["driverName"].ToString(),
                    remarks = model["remarks"].ToString(),
                    amount = null
                };

                ticketing.ticketingId = objGuid;
                ticketing.controlNumber = Convert.ToInt32(model["controlNumber"].ToString());
                ticketing.issuingClerk = info.FullName;
                _context.Ticketing.Update(ticketing);

                var currentTicketing = _context.CurrentTicket.Where(x=> x.ticketingId == objGuid).FirstOrDefault();
                if (currentTicketing.typeOfTransaction != model["typeOfTransaction"].ToString())
                {
                    if (model["typeOfTransaction"].ToString() == "Trader truck")
                    {
                        TradersTruck tradersTruck = new TradersTruck
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            TraderName = "",
                            Destination = ""
                        };
                        _context.TradersTruck.Add(tradersTruck);
                        if (currentTicketing.typeOfTransaction == "Farmer truck")
                        {
                            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (farmersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(farmersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Short trip")
                        {
                            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (shortTrip == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(shortTrip);
                        }
                        else if (currentTicketing.typeOfTransaction == "Pay parking")
                        {
                            PayParking payParking = _context.PayParking.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (payParking == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(payParking);
                        }
                        else if (currentTicketing.typeOfTransaction == "Gate pass")
                        {
                            GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (gatePass == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(gatePass);
                        }
                    }
                    else if (model["typeOfTransaction"].ToString() == "Farmer truck")
                    {
                        FarmersTruck farmersTruck = new FarmersTruck
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            StallNumber = "",
                            FarmersName = "",
                            Organization = "",
                            Commodity = "",
                            Barangay = ""
                        };
                        _context.FarmersTruck.Add(farmersTruck);
                        if (currentTicketing.typeOfTransaction == "Trader truck")
                        {
                            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (tradersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(tradersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Short trip")
                        {
                            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (shortTrip == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(shortTrip);
                        }
                        else if (currentTicketing.typeOfTransaction == "Pay parking")
                        {
                            PayParking payParking = _context.PayParking.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (payParking == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(payParking);
                        }
                        else if (currentTicketing.typeOfTransaction == "Gate pass")
                        {
                            GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (gatePass == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(gatePass);
                        }
                    }

                    else if (model["typeOfTransaction"].ToString() == "Short trip")
                    {
                        ShortTrip shortTrip = new ShortTrip
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            Commodity = ""
                        };
                        _context.ShortTrip.Add(shortTrip);
                        if (currentTicketing.typeOfTransaction == "Trader truck")
                        {
                            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (tradersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(tradersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Farmer truck")
                        {
                            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (farmersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(farmersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Pay parking")
                        {
                            PayParking payParking = _context.PayParking.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (payParking == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(payParking);
                        }
                        else if (currentTicketing.typeOfTransaction == "Gate pass")
                        {
                            GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (gatePass == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(gatePass);
                        }
                    }

                    else if (model["typeOfTransaction"].ToString() == "Pay parking")
                    {
                        PayParking payParking = new PayParking
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            DriverName = ticketing.driverName
                        };
                        _context.PayParking.Add(payParking);
                        if (currentTicketing.typeOfTransaction == "Trader truck")
                        {
                            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (tradersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(tradersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Farmer truck")
                        {
                            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (farmersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(farmersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Short trip")
                        {
                            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (shortTrip == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(shortTrip);
                        }
                        else if (currentTicketing.typeOfTransaction == "Gate pass")
                        {
                            GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (gatePass == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(gatePass);
                        }
                    }

                    else if (model["typeOfTransaction"].ToString() == "Gate pass")
                    {
                        GatePass gatePass = new GatePass
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            DriverName = ticketing.driverName
                        };
                        _context.GatePass.Add(gatePass);
                        if (currentTicketing.typeOfTransaction == "Trader truck")
                        {
                            TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (tradersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(tradersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Farmer truck")
                        {
                            FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (farmersTruck == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(farmersTruck);
                        }
                        else if (currentTicketing.typeOfTransaction == "Short trip")
                        {
                            ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (shortTrip == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(shortTrip);
                        }
                        else if (currentTicketing.typeOfTransaction == "Pay parking")
                        {
                            PayParking payParking = _context.PayParking.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                            if (payParking == null)
                            {
                                return Json(new { success = false, message = "Edit limit is reached!" });
                            }
                            _context.Remove(payParking);
                        }
                    }
                }
                else
                {
                    if (ticketing.typeOfTransaction == "Trader truck")
                    {
                        TradersTruck tradersTruck = new TradersTruck
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            TraderName = "",
                            Destination = ""
                        };
                        _context.TradersTruck.Update(tradersTruck);
                        TradersTruck checkTradersTruck = _context.TradersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                        if (checkTradersTruck == null)
                        {
                            return Json(new { success = false, message = "Edit limit is reached!" });
                        }
                    }
                    else if (ticketing.typeOfTransaction == "Farmer truck")
                    {
                        FarmersTruck farmersTruck = new FarmersTruck
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            StallNumber = "",
                            FarmersName = "",
                            Organization = "",
                            Commodity = "",
                            Barangay = ""
                        };
                        _context.FarmersTruck.Update(farmersTruck);
                        FarmersTruck checkFarmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                        if (checkFarmersTruck == null)
                        {
                            return Json(new { success = false, message = "Edit limit is reached!" });
                        }
                    }
                    else if (ticketing.typeOfTransaction == "Short trip")
                    {
                        ShortTrip shortTrip = new ShortTrip
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            Commodity = ""
                        };
                        _context.ShortTrip.Update(shortTrip);
                        ShortTrip checkShortTrip = _context.ShortTrip.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                        if (checkShortTrip == null)
                        {
                            return Json(new { success = false, message = "Edit limit is reached!" });
                        }
                    }
                    else if (ticketing.typeOfTransaction == "Gate pass")
                    {
                        StallLease stallLease = _context.StallLease.Where(x => x.PlateNumber1 == ticketing.plateNumber || x.PlateNumber2 == ticketing.plateNumber).FirstOrDefault();

                        GatePass gatePass = new GatePass
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            DriverName = ticketing.driverName
                        };

                        if (stallLease != null)
                        {
                            if (stallLease.EndDate > DateTime.Now)
                            {
                                _context.GatePass.Update(gatePass);
                            }
                            else
                            {
                                return Json(new { success = false, message = "Your gate pass is expired!" });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = "Vehicle not registered!" });
                        }
                        GatePass checkGatePass = _context.GatePass.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                        if (checkGatePass == null)
                        {
                            return Json(new { success = false, message = "Edit limit is reached!" });
                        }
                    }
                    else
                    {
                        PayParking payParking = new PayParking
                        {
                            ticketingId = ticketing.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = ticketing.plateNumber,
                            DriverName = ticketing.driverName
                        };
                        _context.PayParking.Update(payParking);
                        PayParking checkPayParking = _context.PayParking.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                        if (checkPayParking == null)
                        {
                            return Json(new { success = false, message = "Edit limit is reached!" });
                        }
                    }
                }
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
            var info = await _userManager.GetUserAsync(User);

            if (ticketing.typeOfTransaction == "Trader truck")
            {
                TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                tradersTruck.TimeOut = ticketing.timeOut;
                var lessThan30Minutes = ticketing.timeIn.Value.AddMinutes(30);

                if (ticketing.typeOfCar == "Overnight")
                {
                    DateTime beforeMidnight = ticketing.timeIn.Value;
                    beforeMidnight = new DateTime(beforeMidnight.Year, beforeMidnight.Month, beforeMidnight.Day, 22, 00, 00); //10 o'clock

                    DateTime midnight = ticketing.timeIn.Value.AddDays(1);
                    midnight = new DateTime(midnight.Year, midnight.Month, midnight.Day, 00, 00, 00); //12 o'clock

                    if (ticketing.timeIn >= beforeMidnight && ticketing.timeIn <= midnight)
                    {
                        var pm = ticketing.timeIn.Value.AddDays(1);
                        pm = new DateTime(pm.Year, pm.Month, pm.Day, 04, 00, 00);

                        var extended1Day = pm.AddHours(18);
                        var extended1Day1Night = pm.AddDays(1);
                        var extended2Days1Night = pm.AddDays(1).AddHours(18);
                        var extended2Days2Nights = pm.AddDays(2);
                        var extended3Days2Nights = pm.AddDays(2).AddHours(18);
                        var extended3Days3Nights = pm.AddDays(3);

                        if (ticketing.timeOut <= lessThan30Minutes)
                        {
                            ticketing.amount = 20;
                        }
                        else if (ticketing.timeOut <= pm)
                        {
                            ticketing.amount = 250;
                        }
                        else if (ticketing.timeOut <= extended1Day)
                        {
                            ticketing.amount = 350;
                        }
                        else if (ticketing.timeOut <= extended1Day1Night)
                        {
                            ticketing.amount = 600;
                        }
                        else if (ticketing.timeOut <= extended2Days1Night)
                        {
                            ticketing.amount = 700;
                        }
                        else if (ticketing.timeOut <= extended2Days2Nights)
                        {
                            ticketing.amount = 950;
                        }
                        else if (ticketing.timeOut <= extended3Days2Nights)
                        {
                            ticketing.amount = 1050;
                        }
                        else if (ticketing.timeOut <= extended3Days3Nights)
                        {
                            ticketing.amount = 1300;
                        }
                    }
                    else
                    {
                        var am = ticketing.timeIn.Value;
                        am = new DateTime(am.Year, am.Month, am.Day, 04, 00, 00);

                        var extended1Day = am.AddHours(18);
                        var extended1Day1Night = am.AddDays(1);
                        var extended2Days1Night = am.AddDays(1).AddHours(18);
                        var extended2Days2Nights = am.AddDays(2);
                        var extended3Days2Nights = am.AddDays(2).AddHours(18);
                        var extended3Days3Nights = am.AddDays(3);

                        if (ticketing.timeOut <= lessThan30Minutes)
                        {
                            ticketing.amount = 20;
                        }
                        else if (ticketing.timeOut <= am)
                        {
                            ticketing.amount = 250;
                        }
                        else if (ticketing.timeOut <= extended1Day)
                        {
                            ticketing.amount = 350;
                        }
                        else if (ticketing.timeOut <= extended1Day1Night)
                        {
                            ticketing.amount = 600;
                        }
                        else if (ticketing.timeOut <= extended2Days1Night)
                        {
                            ticketing.amount = 700;
                        }
                        else if (ticketing.timeOut <= extended2Days2Nights)
                        {
                            ticketing.amount = 950;
                        }
                        else if (ticketing.timeOut <= extended3Days2Nights)
                        {
                            ticketing.amount = 1050;
                        }
                        else if (ticketing.timeOut <= extended3Days3Nights)
                        {
                            ticketing.amount = 1300;
                        }
                    }
                }

                else if (ticketing.typeOfCar == "With transaction")
                {
                    var daytimeExpiration = ticketing.timeIn.Value;
                    daytimeExpiration = new DateTime(daytimeExpiration.Year, daytimeExpiration.Month, daytimeExpiration.Day, 22, 00, 00);

                    var extended1Night = daytimeExpiration.AddHours(6);
                    var extended1Day1Night = daytimeExpiration.AddDays(1);
                    var extended1Day2Nights = daytimeExpiration.AddDays(1).AddHours(6);
                    var extended2Days2Nights = daytimeExpiration.AddDays(2);
                    var extended2Days3Nights = daytimeExpiration.AddDays(2).AddHours(6);
                    var extended3Days3Nights = daytimeExpiration.AddDays(3);

                    if (ticketing.timeOut <= lessThan30Minutes)
                    {
                        ticketing.amount = 20;
                    }
                    else if (ticketing.timeOut <= daytimeExpiration)
                    {
                    ticketing.amount = 150;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        ticketing.amount = 400;
                    }
                    else if (ticketing.timeOut <= extended1Day1Night)
                    {
                        ticketing.amount = 550;
                    }
                    else if (ticketing.timeOut <= extended1Day2Nights)
                    {
                        ticketing.amount = 800;
                    }
                    else if (ticketing.timeOut <= extended2Days2Nights)
                    {
                        ticketing.amount = 950;
                    }
                    else if (ticketing.timeOut <= extended2Days3Nights)
                    {
                        ticketing.amount = 1200;
                    }
                    else if (ticketing.timeOut <= extended3Days3Nights)
                    {
                        ticketing.amount = 1350;
                    }
                }

                else if (ticketing.typeOfCar == "Without transaction")
                {
                    var daytimeExpiration = ticketing.timeIn.Value;
                    daytimeExpiration = new DateTime(daytimeExpiration.Year, daytimeExpiration.Month, daytimeExpiration.Day, 22, 00, 00);

                    var extended1Night = daytimeExpiration.AddHours(6);
                    var extended1Day1Night = daytimeExpiration.AddDays(1);
                    var extended1Day2Nights = daytimeExpiration.AddDays(1).AddHours(6);
                    var extended2Days2Nights = daytimeExpiration.AddDays(2);
                    var extended2Days3Nights = daytimeExpiration.AddDays(2).AddHours(6);
                    var extended3Days3Nights = daytimeExpiration.AddDays(3);

                    if (ticketing.timeOut <= lessThan30Minutes)
                    {
                        ticketing.amount = 20;
                    }
                    else if (ticketing.timeOut <= daytimeExpiration)
                    {
                        ticketing.amount = 100;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        ticketing.amount = 350;
                    }
                    else if (ticketing.timeOut <= extended1Day1Night)
                    {
                        ticketing.amount = 450;
                    }
                    else if (ticketing.timeOut <= extended1Day2Nights)
                    {
                        ticketing.amount = 700;
                    }
                    else if (ticketing.timeOut <= extended2Days2Nights)
                    {
                        ticketing.amount = 800;
                    }
                    else if (ticketing.timeOut <= extended2Days3Nights)
                    {
                        ticketing.amount = 1050;
                    }
                    else if (ticketing.timeOut <= extended3Days3Nights)
                    {
                        ticketing.amount = 1150;
                    }
                }

                _context.TradersTruck.Update(tradersTruck);
            }
            else if (ticketing.typeOfTransaction == "Farmer truck")
            {
                FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                farmersTruck.TimeOut = ticketing.timeOut;

                var day1 = ticketing.timeIn.Value.AddDays(1);
                var day2 = ticketing.timeIn.Value.AddDays(2);
                var day3 = ticketing.timeIn.Value.AddDays(3);
                var day4 = ticketing.timeIn.Value.AddDays(4);
                var day5 = ticketing.timeIn.Value.AddDays(5);
                var day6 = ticketing.timeIn.Value.AddDays(6);
                var day7 = ticketing.timeIn.Value.AddDays(7);
                var day8 = ticketing.timeIn.Value.AddDays(8);
                var day9 = ticketing.timeIn.Value.AddDays(9);
                var day10 = ticketing.timeIn.Value.AddDays(10);

                if (ticketing.typeOfCar == "Single tire")
                {
                    if (ticketing.timeOut <= day1)
                    {
                        ticketing.amount = 100;
                    }
                    else if (ticketing.timeOut <= day2)
                    {
                        ticketing.amount = 200;
                    }
                    else if (ticketing.timeOut <= day3)
                    {
                        ticketing.amount = 300;
                    }
                    else if (ticketing.timeOut <= day4)
                    {
                        ticketing.amount = 400;
                    }
                    else if (ticketing.timeOut <= day5)
                    {
                        ticketing.amount = 500;
                    }
                    else if (ticketing.timeOut <= day6)
                    {
                        ticketing.amount = 600;
                    }
                    else if (ticketing.timeOut <= day7)
                    {
                        ticketing.amount = 700;
                    }
                    else if (ticketing.timeOut <= day8)
                    {
                        ticketing.amount = 800;
                    }
                    else if (ticketing.timeOut <= day9)
                    {
                        ticketing.amount = 900;
                    }
                    else if (ticketing.timeOut <= day10)
                    {
                        ticketing.amount = 1000;
                    }
                }
                else if (ticketing.typeOfCar == "Double tire")
                {
                    if (ticketing.timeOut <= day1)
                    {
                        ticketing.amount = 150;
                    }
                    else if (ticketing.timeOut <= day2)
                    {
                        ticketing.amount = 300;
                    }
                    else if (ticketing.timeOut <= day3)
                    {
                        ticketing.amount = 450;
                    }
                    else if (ticketing.timeOut <= day4)
                    {
                        ticketing.amount = 600;
                    }
                    else if (ticketing.timeOut <= day5)
                    {
                        ticketing.amount = 750;
                    }
                    else if (ticketing.timeOut <= day6)
                    {
                        ticketing.amount = 900;
                    }
                    else if (ticketing.timeOut <= day7)
                    {
                        ticketing.amount = 1050;
                    }
                    else if (ticketing.timeOut <= day8)
                    {
                        ticketing.amount = 1200;
                    }
                    else if (ticketing.timeOut <= day9)
                    {
                        ticketing.amount = 1350;
                    }
                    else if (ticketing.timeOut <= day10)
                    {
                        ticketing.amount = 1500;
                    }
                }
                else if (ticketing.typeOfCar == "Forward tire")
                {
                    if (ticketing.timeOut <= day1)
                    {
                        ticketing.amount = 200;
                    }
                    else if (ticketing.timeOut <= day2)
                    {
                        ticketing.amount = 400;
                    }
                    else if (ticketing.timeOut <= day3)
                    {
                        ticketing.amount = 600;
                    }
                    else if (ticketing.timeOut <= day4)
                    {
                        ticketing.amount = 800;
                    }
                    else if (ticketing.timeOut <= day5)
                    {
                        ticketing.amount = 1000;
                    }
                    else if (ticketing.timeOut <= day6)
                    {
                        ticketing.amount = 1200;
                    }
                    else if (ticketing.timeOut <= day7)
                    {
                        ticketing.amount = 1400;
                    }
                    else if (ticketing.timeOut <= day8)
                    {
                        ticketing.amount = 1600;
                    }
                    else if (ticketing.timeOut <= day9)
                    {
                        ticketing.amount = 1800;
                    }
                    else if (ticketing.timeOut <= day10)
                    {
                        ticketing.amount = 2000;
                    }
                }

                _context.FarmersTruck.Update(farmersTruck);
            }
            else if (ticketing.typeOfTransaction == "Short trip")
            {
                ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
                shortTrip.TimeOut = ticketing.timeOut;

                if (ticketing.typeOfCar == "Pick-up" || ticketing.typeOfCar == "Delivery")
                {
                    if (ticketing.timeIn.Value.AddHours(1) >= ticketing.timeOut)
                    {
                        ticketing.amount = 20;
                    }
                    else if (ticketing.timeIn.Value.AddHours(2) >= ticketing.timeOut)
                    {
                        ticketing.amount = 40;
                    }
                    else if (ticketing.timeIn.Value.AddHours(3) >= ticketing.timeOut)
                    {
                        ticketing.amount = 60;
                    }
                    else if (ticketing.timeIn.Value.AddHours(4) >= ticketing.timeOut)
                    {
                        ticketing.amount = 80;
                    }
                    else if (ticketing.timeIn.Value.AddHours(5) >= ticketing.timeOut)
                    {
                        ticketing.amount = 100;
                    }
                    else if (ticketing.timeIn.Value.AddHours(6) >= ticketing.timeOut)
                    {
                        ticketing.amount = 120;
                    }
                    else if (ticketing.timeIn.Value.AddHours(7) >= ticketing.timeOut)
                    {
                        ticketing.amount = 140;
                    }
                    else if (ticketing.timeIn.Value.AddHours(8) >= ticketing.timeOut)
                    {
                        ticketing.amount = 160;
                    }
                    else if (ticketing.timeIn.Value.AddHours(9) >= ticketing.timeOut)
                    {
                        ticketing.amount = 180;
                    }
                    else if (ticketing.timeIn.Value.AddHours(10) >= ticketing.timeOut)
                    {
                        ticketing.amount = 200;
                    }
                    else if (ticketing.timeIn.Value.AddHours(11) >= ticketing.timeOut)
                    {
                        ticketing.amount = 220;
                    }
                    else if (ticketing.timeIn.Value.AddHours(12) >= ticketing.timeOut)
                    {
                        ticketing.amount = 240;
                    }
                    else if (ticketing.timeIn.Value.AddHours(13) >= ticketing.timeOut)
                    {
                        ticketing.amount = 260;
                    }
                    else if (ticketing.timeIn.Value.AddHours(14) >= ticketing.timeOut)
                    {
                        ticketing.amount = 280;
                    }
                    else if (ticketing.timeIn.Value.AddHours(15) >= ticketing.timeOut)
                    {
                        ticketing.amount = 300;
                    }
                    else if (ticketing.timeIn.Value.AddHours(16) >= ticketing.timeOut)
                    {
                        ticketing.amount = 320;
                    }
                    else if (ticketing.timeIn.Value.AddHours(17) >= ticketing.timeOut)
                    {
                        ticketing.amount = 340;
                    }
                    else if (ticketing.timeIn.Value.AddHours(18) >= ticketing.timeOut)
                    {
                        ticketing.amount = 360;
                    }
                    else if (ticketing.timeIn.Value.AddHours(19) >= ticketing.timeOut)
                    {
                        ticketing.amount = 380;
                    }
                    else if (ticketing.timeIn.Value.AddHours(20) >= ticketing.timeOut)
                    {
                        ticketing.amount = 400;
                    }
                    else if (ticketing.timeIn.Value.AddHours(21) >= ticketing.timeOut)
                    {
                        ticketing.amount = 420;
                    }
                    else if (ticketing.timeIn.Value.AddHours(22) >= ticketing.timeOut)
                    {
                        ticketing.amount = 440;
                    }
                    else if (ticketing.timeIn.Value.AddHours(23) >= ticketing.timeOut)
                    {
                        ticketing.amount = 460;
                    }
                    else if (ticketing.timeIn.Value.AddHours(24) >= ticketing.timeOut)
                    {
                        ticketing.amount = 480;
                    }
                    else if (ticketing.timeIn.Value.AddHours(25) >= ticketing.timeOut)
                    {
                        ticketing.amount = 500;
                    }
                }

                _context.ShortTrip.Update(shortTrip);
            }
            else if (ticketing.typeOfTransaction == "Gate pass")
            {
                GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == id).FirstOrDefault();
                gatePass.TimeOut = ticketing.timeOut;
                _context.GatePass.Update(gatePass);
            }
            else if (ticketing.typeOfTransaction == "Pay parking")
            {
                PayParking payParking = _context.PayParking.Where(x => x.ticketingId == id).FirstOrDefault();
                payParking.TimeOut = ticketing.timeOut;

                if (ticketing.typeOfCar == "Night time")
                {
                    DateTime beforeMidnight = ticketing.timeIn.Value;
                    beforeMidnight = new DateTime(beforeMidnight.Year, beforeMidnight.Month, beforeMidnight.Day, 22, 00, 00); //10 o'clock

                    DateTime midnight = ticketing.timeIn.Value.AddDays(1);
                    midnight = new DateTime(midnight.Year, midnight.Month, midnight.Day, 00, 00, 00); //12 o'clock

                    if (ticketing.timeIn >= beforeMidnight && ticketing.timeIn <= midnight)
                    {
                        var pm = ticketing.timeIn.Value.AddDays(1);
                        pm = new DateTime(pm.Year, pm.Month, pm.Day, 04, 00, 00);

                        var extended1Day = pm.AddHours(18);
                        var extended1Day1Night = pm.AddDays(1);
                        var extended2Days1Night = pm.AddDays(1).AddHours(18);
                        var extended2Days2Nights = pm.AddDays(2);
                        var extended3Days2Nights = pm.AddDays(2).AddHours(18);
                        var extended3Days3Nights = pm.AddDays(3);

                        //if (ticketing.timeOut <= lessThan30Minutes)
                        //{
                        //    ticketing.amount = 20;
                        //}
                        if (ticketing.timeOut <= pm)
                        {
                            ticketing.amount = 100;
                        }
                        else if (ticketing.timeOut <= extended1Day)
                        {
                            ticketing.amount = 140;
                        }
                        else if (ticketing.timeOut <= extended1Day1Night)
                        {
                            ticketing.amount = 240;
                        }
                        else if (ticketing.timeOut <= extended2Days1Night)
                        {
                            ticketing.amount = 280;
                        }
                        else if (ticketing.timeOut <= extended2Days2Nights)
                        {
                            ticketing.amount = 380;
                        }
                        else if (ticketing.timeOut <= extended3Days2Nights)
                        {
                            ticketing.amount = 420;
                        }
                        else if (ticketing.timeOut <= extended3Days3Nights)
                        {
                            ticketing.amount = 520;
                        }
                    }
                    else
                    {
                        var am = ticketing.timeIn.Value;
                        am = new DateTime(am.Year, am.Month, am.Day, 04, 00, 00);

                        var extended1Day = am.AddHours(18);
                        var extended1Day1Night = am.AddDays(1);
                        var extended2Days1Night = am.AddDays(1).AddHours(18);
                        var extended2Days2Nights = am.AddDays(2);
                        var extended3Days2Nights = am.AddDays(2).AddHours(18);
                        var extended3Days3Nights = am.AddDays(3);

                        //if (ticketing.timeOut <= lessThan30Minutes)
                        //{
                        //    ticketing.amount = 20;
                        //}
                        if (ticketing.timeOut <= am)
                        {
                            ticketing.amount = 100;
                        }
                        else if (ticketing.timeOut <= extended1Day)
                        {
                            ticketing.amount = 140;
                        }
                        else if (ticketing.timeOut <= extended1Day1Night)
                        {
                            ticketing.amount = 240;
                        }
                        else if (ticketing.timeOut <= extended2Days1Night)
                        {
                            ticketing.amount = 280;
                        }
                        else if (ticketing.timeOut <= extended2Days2Nights)
                        {
                            ticketing.amount = 380;
                        }
                        else if (ticketing.timeOut <= extended3Days2Nights)
                        {
                            ticketing.amount = 420;
                        }
                        else if (ticketing.timeOut <= extended3Days3Nights)
                        {
                            ticketing.amount = 520;
                        }
                    }
                }

                else if (ticketing.typeOfCar == "Day time")
                {
                    var daytimeExpiration = ticketing.timeIn.Value;
                    daytimeExpiration = new DateTime(daytimeExpiration.Year, daytimeExpiration.Month, daytimeExpiration.Day, 22, 00, 00);

                    var extended1Night = daytimeExpiration.AddHours(6);
                    var extended1Day1Night = daytimeExpiration.AddDays(1);
                    var extended1Day2Nights = daytimeExpiration.AddDays(1).AddHours(6);
                    var extended2Days2Nights = daytimeExpiration.AddDays(2);
                    var extended2Days3Nights = daytimeExpiration.AddDays(2).AddHours(6);
                    var extended3Days3Nights = daytimeExpiration.AddDays(3);

                    //if (ticketing.timeOut <= lessThan30Minutes)
                    //{
                    //    ticketing.amount = 20;
                    //}
                    if (ticketing.timeOut <= daytimeExpiration)
                    {
                        ticketing.amount = 40;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        ticketing.amount = 140;
                    }
                    else if (ticketing.timeOut <= extended1Day1Night)
                    {
                        ticketing.amount = 180;
                    }
                    else if (ticketing.timeOut <= extended1Day2Nights)
                    {
                        ticketing.amount = 280;
                    }
                    else if (ticketing.timeOut <= extended2Days2Nights)
                    {
                        ticketing.amount = 320;
                    }
                    else if (ticketing.timeOut <= extended2Days3Nights)
                    {
                        ticketing.amount = 420;
                    }
                    else if (ticketing.timeOut <= extended3Days3Nights)
                    {
                        ticketing.amount = 460;
                    }
                }

                _context.PayParking.Update(payParking);
            }
            Total total = new Total
            {
                ticketingId = ticketing.ticketingId,
                origin = "Ticketing",
                date = DateTime.Now,
                amount = ticketing.amount.Value,
            };
            ticketing.receivingClerk = info.FullName;
            _context.Total.Add(total);
            _context.Ticketing.Update(ticketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/ExtendGatePass
        [HttpPost("ExtendGatePass")]
        public async Task<IActionResult> ExtendGatePass(Guid id)
        {
            //Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            StallLease stallLease = _context.StallLease.Where(x => x.ticketingId == id).FirstOrDefault();
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();

            //ticketing.timeOut = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            DateTime endDate = new DateTime(currentDate.Year, 12, 31);

            stallLease.EndDate = endDate;
            ticketing.endDate = stallLease.EndDate;
            //gatePass.EndDate = gatePass.EndDate.Value.AddYears(1);
            stallLease.Amount = stallLease.Amount + 500;
            ticketing.amount = ticketing.amount + 500;

            Total total = new Total
            {
                ticketingId = ticketing.ticketingId,
                origin = "Gate pass",
                date = DateTime.Now,
                amount = ticketing.amount.Value
            };

            _context.Total.Add(total);
            _context.StallLease.Update(stallLease);
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
            StallLease stallLease = new StallLease
            {
                StartDate = startDate,
                EndDate = endDate,
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

            if (id == Guid.Empty)
            {
                stallLease.ticketingId = Guid.NewGuid();
                //ticketing.ticketingId = stallLease.ticketingId;

                //gatePass.Id = id();
                _context.StallLease.Add(stallLease);
                //_context.Ticketing.Add(ticketing);
            }
            else
            {
                stallLease.ticketingId = Guid.NewGuid();
                //ticketing.ticketingId = stallLease.ticketingId;
                _context.StallLease.Add(stallLease);
                //_context.Ticketing.Add(ticketing);
            }

            Total total = new Total
            {
                ticketingId = stallLease.ticketingId,
                origin = "Gate pass",
                date = DateTime.Now,
                amount = stallLease.Amount
            };

            _context.Total.Add(total);
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
                PlateNumber = ticketing.plateNumber,
                //PlateNumber2 = ticketing.plateNumber,
                //Status = Convert.ToInt32(model["Status"].ToString()),
                //ContactNumber = model["ContactNumber"].ToString(),
                //IdType = model["IdType"].ToString(),
                //IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Remarks = ticketing.remarks,

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
            if (ticketing.typeOfTransaction == "Trader truck")
            {
                TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                _context.Remove(tradersTruck);
            }
            else if (ticketing.typeOfTransaction == "Farmer truck")
            {
                FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                _context.Remove(farmersTruck);
            }
            else if (ticketing.typeOfTransaction == "Short trip")
            {
                ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
                _context.Remove(shortTrip);
            }
            else if (ticketing.typeOfTransaction == "Pay parking")
            {
                PayParking payParking = _context.PayParking.Where(x => x.ticketingId == id).FirstOrDefault();
                _context.Remove(payParking);
            }
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

    //internal class GetGatePass
    //{
    //}
}