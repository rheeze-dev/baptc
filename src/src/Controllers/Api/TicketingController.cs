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
                    if (ticketing.typeOfCar == "With transaction" || ticketing.typeOfCar == "Without transaction")
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
                    if (ticketing.typeOfCar == "Single tire" || ticketing.typeOfCar == "Double tire")
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
                    if (ticketing.driverName == "")
                    {
                        return Json(new { success = false, message = "Drivers name cannot be empty!" });
                    }
                    ticketing.typeOfCar = "";
                    _context.Ticketing.Add(ticketing);
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
                        Destination = "",
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
                        if (model["typeOfCar"].ToString() != "With transaction" && model["typeOfCar"].ToString() != "Without transaction")
                        {
                            return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                        }
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
                        if (model["typeOfCar"].ToString() != "Single tire" && model["typeOfCar"].ToString() != "Double tire")
                        {
                            return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                        }
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
                        if (model["typeOfCar"].ToString() != "Pick-up" && model["typeOfCar"].ToString() != "Delivery")
                        {
                            return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                        }
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

                        StallLease stallLease = _context.StallLease.Where(x => x.PlateNumber1 == ticketing.plateNumber || x.PlateNumber2 == ticketing.plateNumber).FirstOrDefault();

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
                        if (model["driverName"].ToString() == "")
                        {
                            return Json(new { success = false, message = "Drivers name cannot be empty!" });
                        }
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
            var ticketingPrice = _context.TicketingPrice.FirstOrDefault();
            ticketing.timeOut = DateTime.Now;
            var info = await _userManager.GetUserAsync(User);

            DateTime beforeMidnight = ticketing.timeIn.Value;
            beforeMidnight = new DateTime(beforeMidnight.Year, beforeMidnight.Month, beforeMidnight.Day, 22, 00, 00); //10 o'clock

            var afterMidnight = ticketing.timeIn.Value.AddDays(1);
            afterMidnight = new DateTime(afterMidnight.Year, afterMidnight.Month, afterMidnight.Day, 04, 00, 00);

            DateTime midnight = ticketing.timeIn.Value.AddDays(1);
            midnight = new DateTime(midnight.Year, midnight.Month, midnight.Day, 00, 00, 00); //12 o'clock

            var am = ticketing.timeIn.Value;
            am = new DateTime(am.Year, am.Month, am.Day, 22, 00, 00);

            var extended1Night = am.AddDays(1);
            var extended2Nights = am.AddDays(2);
            var extended3Nights = am.AddDays(3);
            var extended4Nights = am.AddDays(4);
            var extended5Nights = am.AddDays(5);
            var extended6Nights = am.AddDays(6);
            var extended7Nights = am.AddDays(7);

            if (ticketing.typeOfTransaction == "Trader truck")
            {
                TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                tradersTruck.TimeOut = ticketing.timeOut;

                if (ticketing.typeOfCar == "With transaction")
                {
                    if (ticketing.timeIn >= beforeMidnight && ticketing.timeIn <= midnight)
                    {
                        var pm = ticketing.timeIn.Value.AddDays(1);
                        pm = new DateTime(pm.Year, pm.Month, pm.Day, 22, 00, 00);

                        var extended1NightPm = pm.AddDays(1);
                        var extended2NightsPm = pm.AddDays(2);
                        var extended3NightsPm = pm.AddDays(3);
                        var extended4NightsPm = pm.AddDays(4);
                        var extended5NightsPm = pm.AddDays(5);
                        var extended6NightsPm = pm.AddDays(6);

                        if (ticketing.timeOut <= pm)
                        {
                            ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 250;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended1NightPm)
                        {
                            ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 350;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                        }
                        else if (ticketing.timeOut <= extended2NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 450;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                        }
                        else if (ticketing.timeOut <= extended3NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 550;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                        }
                        else if (ticketing.timeOut <= extended4NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 650;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                        }
                        else if (ticketing.timeOut <= extended5NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 750;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                        }
                        else if (ticketing.timeOut <= extended6NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 850;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                        }
                    }
                    else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                    {
                        if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                        {
                            if (ticketing.timeOut <= am)
                            {
                                //ticketing.amount = 150;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction;
                            }
                            else if (ticketing.timeOut <= extended1Night)
                            {
                            ticketing.typeOfCar = "Overnight(1) with transaction";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended2Nights)
                            {
                            ticketing.typeOfCar = "Overnight(2) with transaction";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended3Nights)
                            {
                            ticketing.typeOfCar = "Overnight(3) with transaction";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended4Nights)
                            {
                            ticketing.typeOfCar = "Overnight(4) with transaction";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended5Nights)
                            {
                            ticketing.typeOfCar = "Overnight(5) with transaction";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended6Nights)
                            {
                            ticketing.typeOfCar = "Overnight(6) with transaction";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended7Nights)
                            {
                                ticketing.typeOfCar = "Overnight(7) with transaction";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                        else if (ticketing.timeOut <= am)
                        {
                            ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 250;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 350;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 450;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 550;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 650;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 750;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 850;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                        }
                    }
                }

                else if (ticketing.typeOfCar == "Without transaction")
                {
                    if (ticketing.timeIn >= beforeMidnight && ticketing.timeIn <= midnight)
                    {
                        var pm = ticketing.timeIn.Value.AddDays(1);
                        pm = new DateTime(pm.Year, pm.Month, pm.Day, 22, 00, 00);

                        var extended1NightPm = pm.AddDays(1);
                        var extended2NightsPm = pm.AddDays(2);
                        var extended3NightsPm = pm.AddDays(3);
                        var extended4NightsPm = pm.AddDays(4);
                        var extended5NightsPm = pm.AddDays(5);
                        var extended6NightsPm = pm.AddDays(6);

                        if (ticketing.timeOut <= pm)
                        {
                            ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 250;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended1NightPm)
                        {
                            ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 350;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                        }
                        else if (ticketing.timeOut <= extended2NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 450;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                        }
                        else if (ticketing.timeOut <= extended3NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 550;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                        }
                        else if (ticketing.timeOut <= extended4NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 650;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                        }
                        else if (ticketing.timeOut <= extended5NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 750;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                        }
                        else if (ticketing.timeOut <= extended6NightsPm)
                        {
                            ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 850;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                        }
                    }
                    else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                    {
                        if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                        {
                            if (ticketing.timeOut <= am)
                            {
                                //ticketing.amount = 100;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction;
                            }
                            else if (ticketing.timeOut <= extended1Night)
                            {
                                ticketing.typeOfCar = "Overnight(1) without transaction";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended2Nights)
                            {
                                ticketing.typeOfCar = "Overnight(2) without transaction";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended3Nights)
                            {
                                ticketing.typeOfCar = "Overnight(3) without transaction";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended4Nights)
                            {
                                ticketing.typeOfCar = "Overnight(4) without transaction";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended5Nights)
                            {
                                ticketing.typeOfCar = "Overnight(5) without transaction";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended6Nights)
                            {
                                ticketing.typeOfCar = "Overnight(6) without transaction";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended7Nights)
                            {
                                ticketing.typeOfCar = "Overnight(7) without transaction";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                        else if (ticketing.timeOut <= am)
                        {
                            ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 250;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 350;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 450;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 550;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 650;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 750;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 850;
                            ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                        }
                    }
                }

                _context.TradersTruck.Update(tradersTruck);
            }
            else if (ticketing.typeOfTransaction == "Farmer truck")
            {
                FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                farmersTruck.TimeOut = ticketing.timeOut;

                if (ticketing.typeOfCar == "Single tire")
                {
                    if (ticketing.timeOut <= extended1Night)
                    {
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 2);
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 3);
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 4);
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 5);
                    }
                    else if (ticketing.timeOut <= extended7Nights)
                    {
                        ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 6);
                    }
                }
                else if (ticketing.typeOfCar == "Double tire")
                {
                    if (ticketing.timeOut <= extended1Night)
                    {
                        //ticketing.amount = 150;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 250;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 350;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 2);
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 450;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 3);
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 550;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 4);
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 650;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 5);
                    }
                    else if (ticketing.timeOut <= extended7Nights)
                    {
                        ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 750;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 6);
                    }
                }

                _context.FarmersTruck.Update(farmersTruck);
            }
            else if (ticketing.typeOfTransaction == "Short trip")
            {
                ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
                shortTrip.TimeOut = ticketing.timeOut;

                var AM = ticketing.timeIn.Value;
                AM = new DateTime(AM.Year, AM.Month, AM.Day, 00, 00, 00);

                var pm = ticketing.timeIn.Value;
                pm = new DateTime(pm.Year, pm.Month, pm.Day, 12, 00, 00);
                if (ticketing.timeIn >= pm && ticketing.timeIn <= midnight)
                {
                    if (ticketing.typeOfCar == "Pick-up" || ticketing.typeOfCar == "Delivery")
                    {
                        if (midnight >= ticketing.timeOut)
                        {
                            //ticketing.amount = 20;
                            ticketing.amount = ticketingPrice.ShortTripPickUp;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            //ticketing.amount = 100;
                            ticketing.typeOfCar = "Overnight(1)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.amount = 200;
                            ticketing.typeOfCar = "Overnight(2)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.amount = 300;
                            ticketing.typeOfCar = "Overnight(3)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.amount = 400;
                            ticketing.typeOfCar = "Overnight(4)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.amount = 500;
                            ticketing.typeOfCar = "Overnight(5)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.amount = 600;
                            ticketing.typeOfCar = "Overnight(6)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended7Nights)
                        {
                            //ticketing.amount = 700;
                            ticketing.typeOfCar = "Overnight(7)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }

                    }
                }
                else if (ticketing.timeIn >= AM && ticketing.timeIn <= pm)
                {
                    if (ticketing.typeOfCar == "Pick-up" || ticketing.typeOfCar == "Delivery")
                    {
                        if (midnight.AddHours(-12) >= ticketing.timeOut)
                        {
                            //ticketing.amount = 20;
                            ticketing.amount = ticketingPrice.ShortTripPickUp;
                        }
                        else if (midnight >= ticketing.timeOut)
                        {
                            //ticketing.amount = 40;
                            ticketing.amount = ticketingPrice.ShortTripPickUp * 2;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            //ticketing.amount = 100;
                            ticketing.typeOfCar = "Overnight(1)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.amount = 200;
                            ticketing.typeOfCar = "Overnight(2)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.amount = 300;
                            ticketing.typeOfCar = "Overnight(3)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.amount = 400;
                            ticketing.typeOfCar = "Overnight(4)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.amount = 500;
                            ticketing.typeOfCar = "Overnight(5)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.amount = 600;
                            ticketing.typeOfCar = "Overnight(6)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended7Nights)
                        {
                            //ticketing.amount = 700;
                            ticketing.typeOfCar = "Overnight(7)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }
                    }
                }

                _context.ShortTrip.Update(shortTrip);
            }
            else if (ticketing.typeOfTransaction == "Gate pass")
            {
                GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == id).FirstOrDefault();
                gatePass.TimeOut = ticketing.timeOut;
                _context.GatePass.Update(gatePass);

                if (ticketing.timeIn >= beforeMidnight && ticketing.timeIn <= midnight)
                {
                    var pm = ticketing.timeIn.Value.AddDays(1);
                    pm = new DateTime(pm.Year, pm.Month, pm.Day, 22, 00, 00);

                    var extended1NightPm = pm.AddDays(1);
                    var extended2NightsPm = pm.AddDays(2);
                    var extended3NightsPm = pm.AddDays(3);
                    var extended4NightsPm = pm.AddDays(4);
                    var extended5NightsPm = pm.AddDays(5);
                    var extended6NightsPm = pm.AddDays(6);

                    if (ticketing.timeOut <= pm)
                    {
                        ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1NightPm)
                    {
                        ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                    }
                }
                else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                {
                    if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                    {
                        if (ticketing.timeOut <= am)
                        {
                            //ticketing.amount = 0;
                            ticketing.typeOfCar = "Gate pass";
                            ticketing.amount = 0;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 100;
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 200;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 300;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 400;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 500;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 600;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended6Nights.AddDays(1))
                        {
                            ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 700;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }
                    }
                    else if (ticketing.timeOut <= am)
                    {
                        ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                    }
                }

            }
            else if (ticketing.typeOfTransaction == "Pay parking")
            {
                PayParking payParking = _context.PayParking.Where(x => x.ticketingId == id).FirstOrDefault();
                payParking.TimeOut = ticketing.timeOut;

                if (ticketing.timeIn >= beforeMidnight && ticketing.timeIn <= midnight)
                {
                    var pm = ticketing.timeIn.Value.AddDays(1);
                    pm = new DateTime(pm.Year, pm.Month, pm.Day, 22, 00, 00);

                    var extended1NightPm = pm.AddDays(1);
                    var extended2NightsPm = pm.AddDays(2);
                    var extended3NightsPm = pm.AddDays(3);
                    var extended4NightsPm = pm.AddDays(4);
                    var extended5NightsPm = pm.AddDays(5);
                    var extended6NightsPm = pm.AddDays(6);

                    if (ticketing.timeOut <= pm)
                    {
                        ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1NightPm)
                    {
                        ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6NightsPm)
                    {
                        ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                    }
                }
                else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                {
                    if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                    {
                        if (ticketing.timeOut <= am)
                        {
                            ticketing.typeOfCar = "Day time";
                            //ticketing.amount = 40;
                            ticketing.amount = ticketingPrice.PayParkingDaytime;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 100;
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 200;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 300;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 400;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 500;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 600;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended6Nights.AddDays(1))
                        {
                            ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 700;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }
                    }
                    else if (ticketing.timeOut <= am)
                    {
                        ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
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
            var amount = ticketing.amount;
            _context.Total.Add(total);
            _context.Ticketing.Update(ticketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Your payment amount is " + amount });
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

        // POST: api/Ticketing/ExtendGatePass
        [HttpPost("AddCount")]
        public async Task<IActionResult> AddCount(Guid id)
        {
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();

            if (ticketing.count == null)
            {
                ticketing.count = 1;
            }
            else if (ticketing.count != null)
            {
                ticketing.count = ticketing.count.Value + 1;
            }


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

        // GET: api/Ticketing/GetParkingNumber
        [HttpGet("GetParkingNumber")]
        public IActionResult GetParkingNumber([FromRoute]Guid organizationId)
        {
            var listParking = _context.ParkingNumbers.Where(x => x.Selected == false).ToList();

            return Json(new { data = listParking });
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


}