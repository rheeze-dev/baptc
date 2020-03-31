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
                _context.Ticketing.Add(ticketing);

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
                    if (ticketing.typeOfTransaction == "Trader truck" && ticketing.typeOfCar == "Single tire")
                    {
                        ticketing.amount = 50;
                    }
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
                _context.TradersTruck.Update(tradersTruck);
            }
            else if (ticketing.typeOfTransaction == "Farmer truck")
            {
                FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                farmersTruck.TimeOut = ticketing.timeOut;
                _context.FarmersTruck.Update(farmersTruck);
            }
            else if (ticketing.typeOfTransaction == "Short trip")
            {
                ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
                shortTrip.TimeOut = ticketing.timeOut;
                _context.ShortTrip.Update(shortTrip);
            }
            else if (ticketing.typeOfTransaction == "Gate pass")
            {
                GatePass gatePass = _context.GatePass.Where(x => x.ticketingId == id).FirstOrDefault();
                gatePass.TimeOut = ticketing.timeOut;
                _context.GatePass.Update(gatePass);
            }
            else
            {
                PayParking payParking = _context.PayParking.Where(x => x.ticketingId == id).FirstOrDefault();
                payParking.TimeOut = ticketing.timeOut;
                _context.PayParking.Update(payParking);
            }
            ticketing.receivingClerk = info.FullName;
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


            //_context.Ticketing.Update(ticketing);
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

            //GatePass gatePass = new GatePass
            //{
            //    ticketingId = stallLease.ticketingId,
            //    amount = stallLease.Amount,
            //    plateNumber = stallLease.PlateNumber1 + ", " + stallLease.PlateNumber2,
            //    endDate = stallLease.EndDate,
            //    driverName = stallLease.DriverName
            //};
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