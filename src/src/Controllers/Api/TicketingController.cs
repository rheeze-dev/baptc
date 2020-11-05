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
                    TypeOfEntry = model["TypeOfEntry"].ToString(),
                    driverName = model["driverName"].ToString(),
                    remarks = model["remarks"].ToString(),
                    amount = null,
                    ContactNumber = model["ContactNumber"].ToString(),
                    Address = model["Address"].ToString(),
                    Temperature = Convert.ToDouble(model["Temperature"].ToString())
                };
                ticketing.Transaction = "Unfinished";

                var credit = _context.Ticketing.Where(x => x.plateNumber == model["plateNumber"].ToString()).OrderByDescending(x => x.timeIn).Select(x => x.TotalCredit).FirstOrDefault();
                ticketing.TotalCredit = credit;

                Buyers buyers = _context.AccreditedBuyers.Where(x => x.VehiclePlateNumber == model["plateNumber"].ToString()).FirstOrDefault();
                IndividualFarmers individualFarmers = _context.AccreditedIndividualFarmers.Where(x => x.PlateNumber == model["plateNumber"].ToString()).FirstOrDefault();
                MarketFacilitators marketFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.PlateNumber == model["plateNumber"].ToString()).FirstOrDefault();
                if (buyers != null)
                {
                    ticketing.accreditation = "Buyer";
                    var reset = _context.DailyBuyers.OrderByDescending(x => x.Date).Select(x => x.Date.Day).FirstOrDefault();
                    var lastCount = _context.DailyBuyers.OrderByDescending(x => x.Date).Select(x => x.Count).FirstOrDefault();

                    DailyBuyers dailyBuyers = new DailyBuyers
                    {
                        Date = DateTime.Now,
                        PlateNumber = model["plateNumber"].ToString(),
                    };
                    if (DateTime.Now.Day == reset)
                    {
                        dailyBuyers.Count = lastCount + 1;
                        _context.DailyBuyers.Add(dailyBuyers);

                        TotalBuyers totalBuyers = new TotalBuyers
                        {
                            Date = dailyBuyers.Date,
                            Total = dailyBuyers.Count
                        };
                        if (dailyBuyers.Count == 1)
                        {
                            _context.TotalBuyers.Add(totalBuyers);
                        }
                        else
                        {
                            TotalBuyers currentTotalBuyers = _context.TotalBuyers.Where(x => x.Date.Day == DateTime.Now.Day).FirstOrDefault();
                            currentTotalBuyers.Total = dailyBuyers.Count;
                            _context.TotalBuyers.Update(currentTotalBuyers);
                        }

                    }

                    else if (DateTime.Now.Day != reset)
                    {
                        dailyBuyers.Count = 1;
                        _context.DailyBuyers.Add(dailyBuyers);

                        TotalBuyers totalBuyers = new TotalBuyers
                        {
                            Date = dailyBuyers.Date,
                            Total = dailyBuyers.Count
                        };
                        if (dailyBuyers.Count == 1)
                        {
                            _context.TotalBuyers.Add(totalBuyers);
                        }
                        else
                        {
                            TotalBuyers currentTotalBuyers = _context.TotalBuyers.Where(x => x.Date.Day == DateTime.Now.Day).FirstOrDefault();
                            currentTotalBuyers.Total = dailyBuyers.Count;
                            _context.TotalBuyers.Update(currentTotalBuyers);
                        }
                    }
                }
                else if (individualFarmers != null)
                {
                    ticketing.accreditation = "Farmer";
                    var reset = _context.DailyFarmers.OrderByDescending(x => x.Date).Select(x => x.Date.Day).FirstOrDefault();
                    var lastCount = _context.DailyFarmers.OrderByDescending(x => x.Date).Select(x => x.Count).FirstOrDefault();

                    DailyFarmers dailyFarmers = new DailyFarmers
                    {
                        Date = DateTime.Now,
                        PlateNumber = model["plateNumber"].ToString(),
                    };
                    if (DateTime.Now.Day == reset)
                    {
                        dailyFarmers.Count = lastCount + 1;
                        _context.DailyFarmers.Add(dailyFarmers);

                        TotalFarmers totalFarmers = new TotalFarmers
                        {
                            Date = dailyFarmers.Date,
                            Total = dailyFarmers.Count
                        };
                        if (dailyFarmers.Count == 1)
                        {
                            _context.TotalFarmers.Add(totalFarmers);
                        }
                        else
                        {
                            TotalFarmers currentTotalFarmers = _context.TotalFarmers.Where(x => x.Date.Day == DateTime.Now.Day).FirstOrDefault();
                            currentTotalFarmers.Total = dailyFarmers.Count;
                            _context.TotalFarmers.Update(currentTotalFarmers);
                        }

                    }

                    else if (DateTime.Now.Day != reset)
                    {
                        dailyFarmers.Count = 1;
                        _context.DailyFarmers.Add(dailyFarmers);

                        TotalFarmers totalFarmers = new TotalFarmers
                        {
                            Date = dailyFarmers.Date,
                            Total = dailyFarmers.Count
                        };
                        if (dailyFarmers.Count == 1)
                        {
                            _context.TotalFarmers.Add(totalFarmers);
                        }
                        else
                        {
                            TotalFarmers currentTotalFarmers = _context.TotalFarmers.Where(x => x.Date.Day == DateTime.Now.Day).FirstOrDefault();
                            currentTotalFarmers.Total = dailyFarmers.Count;
                            _context.TotalFarmers.Update(currentTotalFarmers);
                        }
                    }
                }
                else if (marketFacilitators != null)
                {
                    ticketing.accreditation = "Market facilitator";
                    var reset = _context.DailyFacilitators.OrderByDescending(x => x.Date).Select(x => x.Date.Day).FirstOrDefault();
                    var lastCount = _context.DailyFacilitators.OrderByDescending(x => x.Date).Select(x => x.Count).FirstOrDefault();

                    DailyFacilitators dailyFacilitators = new DailyFacilitators
                    {
                        Date = DateTime.Now,
                        PlateNumber = model["plateNumber"].ToString()
                    };
                    if (dailyFacilitators.Date.Day == reset)
                    {
                        dailyFacilitators.Count = lastCount + 1;
                        _context.DailyFacilitators.Add(dailyFacilitators);

                        TotalFacilitators totalFacilitators = new TotalFacilitators
                        {
                            Date = dailyFacilitators.Date,
                            Total = dailyFacilitators.Count
                        };
                        if (dailyFacilitators.Count == 1)
                        {
                            _context.TotalFacilitators.Add(totalFacilitators);
                        }
                        else
                        {
                            TotalFacilitators currentTotalFacilitators = _context.TotalFacilitators.Where(x => x.Date.Day == DateTime.Now.Day).FirstOrDefault();
                            currentTotalFacilitators.Total = dailyFacilitators.Count;
                            _context.TotalFacilitators.Update(currentTotalFacilitators);
                        }

                    }

                    else if (dailyFacilitators.Date.Day != reset)
                    {
                        dailyFacilitators.Count = 1;
                        _context.DailyFacilitators.Add(dailyFacilitators);

                        TotalFacilitators totalFacilitators = new TotalFacilitators
                        {
                            Date = dailyFacilitators.Date,
                            Total = dailyFacilitators.Count
                        };
                        if (dailyFacilitators.Count == 1)
                        {
                            _context.TotalFacilitators.Add(totalFacilitators);
                        }
                        else
                        {
                            TotalFacilitators currentTotalFacilitators = _context.TotalFacilitators.Where(x => x.Date.Day == DateTime.Now.Day).FirstOrDefault();
                            currentTotalFacilitators.Total = dailyFacilitators.Count;
                            _context.TotalFacilitators.Update(currentTotalFacilitators);
                        }
                    }
                }
                else
                {
                    ticketing.accreditation = "";
                }

                if (model["parkingNumber"].ToString() == "")
                {
                    return Json(new { success = false, message = "Parking number cannot be empty!" });
                }
                else
                {
                    ticketing.parkingNumber = model["parkingNumber"].ToString();
                }

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
                    if (ticketing.TypeOfEntry == "With transaction" || ticketing.TypeOfEntry == "Without transaction")
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
                    ticketing.TypeOfEntry = "";
                    if (ticketing.typeOfCar == "Single tire" || ticketing.typeOfCar == "Double tire")
                    {
                        _context.Ticketing.Add(ticketing);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of car!" });
                    }
                }

                else if (ticketing.typeOfTransaction == "Short trip")
                {
                    if (ticketing.TypeOfEntry == "Pick-up" || ticketing.TypeOfEntry == "Delivery")
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

                ParkingNumbers parking = _context.ParkingNumbers.Where(x => x.Name == model["parkingNumber"].ToString()).FirstOrDefault();
                parking.Selected = true;
                _context.ParkingNumbers.Update(parking);

                CurrentTicket currentTicket = new CurrentTicket
                {
                    ticketingId = ticketing.ticketingId,
                    plateNumber = ticketing.plateNumber,
                    typeOfTransaction = ticketing.typeOfTransaction
                };
                _context.CurrentTicket.Add(currentTicket);

                var getLastControlNumberTrader = _context.TradersTruck.OrderByDescending(x => x.ControlNumber).Select(x => x.ControlNumber).FirstOrDefault();
                var getLastControlNumberFarmer = _context.FarmersTruck.OrderByDescending(x => x.ControlNumber).Select(x => x.ControlNumber).FirstOrDefault();
                var getLastControlNumberShort = _context.ShortTrip.OrderByDescending(x => x.ControlNumber).Select(x => x.ControlNumber).FirstOrDefault();

                if (ticketing.typeOfTransaction == "Trader truck")
                {
                    TradersTruck tradersTruck = new TradersTruck
                    {
                        ticketingId = ticketing.ticketingId,
                        TimeIn = DateTime.Now,
                        PlateNumber = ticketing.plateNumber,
                        TraderName = "",
                        Destination = "",
                        ParkingNumber = ticketing.parkingNumber
                    };
                    if (getLastControlNumberTrader == null)
                    {
                        tradersTruck.ControlNumber = 1;
                    }
                    else
                    {
                        tradersTruck.ControlNumber = getLastControlNumberTrader.Value + 1;
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
                        Barangay = "",
                        ParkingNumber = ticketing.parkingNumber
                    };
                    if (getLastControlNumberFarmer == null)
                    {
                        farmersTruck.ControlNumber = 1;
                    }
                    else
                    {
                        farmersTruck.ControlNumber = getLastControlNumberFarmer.Value + 1;
                    }

                    _context.FarmersTruck.Add(farmersTruck);
                }
                else if (ticketing.typeOfTransaction == "Short trip")
                {
                    ShortTrip shortTrip = new ShortTrip
                    {
                        ticketingId = ticketing.ticketingId,
                        TimeIn = DateTime.Now,
                        PlateNumber = ticketing.plateNumber,
                        CommodityIn = "",
                        CommodityOut = "",
                        ParkingNumber = ticketing.parkingNumber,
                        TypeOfEntry = ticketing.typeOfCar
                    };
                    if (getLastControlNumberShort == null)
                    {
                        shortTrip.ControlNumber = 1;
                    }
                    else
                    {
                        shortTrip.ControlNumber = getLastControlNumberShort.Value + 1;
                    }

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
                            DriverName = ticketing.driverName,
                            ParkingNumber = ticketing.parkingNumber
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
                        DriverName = ticketing.driverName,
                        ParkingNumber = ticketing.parkingNumber
                    };

                    _context.PayParking.Add(payParking);
                }
            }
            else
            {
                //Ticketing ticketing = new Ticketing
                //{
                //    timeIn = DateTime.Now,
                //    plateNumber = model["plateNumber"].ToString(),
                //    typeOfTransaction = model["typeOfTransaction"].ToString(),
                //    typeOfCar = model["typeOfCar"].ToString(),
                //    driverName = model["driverName"].ToString(),
                //    remarks = model["remarks"].ToString(),
                //    amount = null
                //};

                Ticketing currentTicketingIn = _context.Ticketing.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                if (currentTicketingIn.plateNumber != model["plateNumber"].ToString() || currentTicketingIn.typeOfTransaction != model["typeOfTransaction"].ToString() || currentTicketingIn.typeOfCar != model["typeOfCar"].ToString() || currentTicketingIn.parkingNumber != model["parkingNumber"].ToString() || currentTicketingIn.driverName != model["driverName"].ToString() || currentTicketingIn.ContactNumber != model["ContactNumber"].ToString() || currentTicketingIn.Address != model["Address"].ToString() || currentTicketingIn.Temperature != Convert.ToInt32(model["Temperature"].ToString()) || currentTicketingIn.remarks != model["remarks"].ToString() || currentTicketingIn.TypeOfEntry != model["TypeOfEntry"].ToString())
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
                    var ten = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Ticketing in",
                        EditedBy = info.FullName,
                        ControlNumber = currentTicketingIn.controlNumber.Value
                    };
                    if (currentTicketingIn.plateNumber != model["plateNumber"].ToString())
                    {
                        one = "Plate number = " + currentTicketingIn.plateNumber + " - " + model["plateNumber"].ToString() + "; ";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentTicketingIn.typeOfTransaction != model["typeOfTransaction"].ToString())
                    {
                        two = " Type of transaction = " + currentTicketingIn.typeOfTransaction + " - " + model["typeOfTransaction"].ToString() + "; ";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentTicketingIn.TypeOfEntry != model["TypeOfEntry"].ToString())
                    {
                        three = " Type of entry = " + currentTicketingIn.TypeOfEntry + " - " + model["TypeOfEntry"].ToString() + "; ";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentTicketingIn.typeOfCar != model["typeOfCar"].ToString())
                    {
                        four = " Type of car = " + currentTicketingIn.typeOfCar + " - " + model["typeOfCar"].ToString() + "; ";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentTicketingIn.parkingNumber != model["parkingNumber"].ToString())
                    {
                        five = " Parking number = " + currentTicketingIn.parkingNumber + " - " + model["parkingNumber"].ToString() + "; ";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentTicketingIn.driverName != model["driverName"].ToString())
                    {
                        six = " Drivers name = " + currentTicketingIn.driverName + " - " + model["driverName"].ToString() + "; ";
                    }
                    else
                    {
                        six = "";
                    }
                    if (currentTicketingIn.ContactNumber != model["ContactNumber"].ToString())
                    {
                        seven = " Contact number = " + currentTicketingIn.ContactNumber + " - " + model["ContactNumber"].ToString() + "; ";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (currentTicketingIn.Address != model["Address"].ToString())
                    {
                        eight = " Address = " + currentTicketingIn.Address + " - " + model["Address"].ToString() + "; ";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (currentTicketingIn.Temperature != Convert.ToInt32(model["Temperature"].ToString()))
                    {
                        nine = " Temperature = " + currentTicketingIn.Temperature + " - " + Convert.ToInt32(model["Temperature"].ToString()) + "; ";
                    }
                    else
                    {
                        nine = "";
                    }
                    if (currentTicketingIn.remarks != model["remarks"].ToString())
                    {
                        ten = " Remarks = " + currentTicketingIn.remarks + " - " + model["remarks"].ToString() + "; ";
                    }
                    else
                    {
                        ten = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine + ten;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                var currentParkingNumber = currentTicketingIn.parkingNumber;
                currentTicketingIn.plateNumber = model["plateNumber"].ToString();
                currentTicketingIn.typeOfTransaction = model["typeOfTransaction"].ToString();
                currentTicketingIn.TypeOfEntry = model["TypeOfEntry"].ToString();
                currentTicketingIn.typeOfCar = model["typeOfCar"].ToString();
                currentTicketingIn.driverName = model["driverName"].ToString();
                currentTicketingIn.ContactNumber = model["ContactNumber"].ToString();
                currentTicketingIn.Address = model["Address"].ToString();
                currentTicketingIn.Temperature = Convert.ToInt32(model["Temperature"].ToString());
                currentTicketingIn.remarks = model["remarks"].ToString();
                if (model["parkingNumber"].ToString() == "")
                {
                    currentTicketingIn.parkingNumber = currentParkingNumber;
                }
                else
                {
                    ParkingNumbers originalParking = _context.ParkingNumbers.Where(x => x.Name == currentParkingNumber).FirstOrDefault();
                    originalParking.Selected = false;
                    _context.ParkingNumbers.Update(originalParking);

                    ParkingNumbers newParking = _context.ParkingNumbers.Where(x => x.Name == model["parkingNumber"].ToString()).FirstOrDefault();
                    newParking.Selected = true;
                    _context.ParkingNumbers.Update(newParking);

                    currentTicketingIn.parkingNumber = model["parkingNumber"].ToString();
                }

                if (currentTicketingIn.typeOfTransaction == "Trader truck")
                {
                    if (currentTicketingIn.TypeOfEntry != "With transaction" && currentTicketingIn.TypeOfEntry != "Without transaction")
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                    }
                }

                else if (currentTicketingIn.typeOfTransaction == "Farmer truck")
                {
                    currentTicketingIn.TypeOfEntry = "";
                    if (currentTicketingIn.typeOfCar != "Single tire" && currentTicketingIn.typeOfCar != "Double tire")
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of car!" });
                    }
                }

                else if (currentTicketingIn.typeOfTransaction == "Short trip")
                {
                    if (currentTicketingIn.TypeOfEntry != "Pick-up" && currentTicketingIn.TypeOfEntry != "Delivery")
                    {
                        return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                    }
                }

                else if (currentTicketingIn.typeOfTransaction == "Pay parking")
                {
                    if (currentTicketingIn.driverName == "")
                    {
                        return Json(new { success = false, message = "Drivers name cannot be empty!" });
                    }
                    currentTicketingIn.typeOfCar = "";
                }
                else if (currentTicketingIn.typeOfTransaction == "Gate pass")
                {
                    if (currentTicketingIn.driverName == "")
                    {
                        return Json(new { success = false, message = "Drivers name cannot be empty!" });
                    }
                    currentTicketingIn.typeOfCar = "";
                }

                _context.Ticketing.Update(currentTicketingIn);

                //ticketing.ticketingId = objGuid;
                //ticketing.controlNumber = Convert.ToInt32(model["controlNumber"].ToString());
                //ticketing.issuingClerk = info.FullName;
                //ticketing.parkingNumber = model["parkingNumber"].ToString();
                //_context.Ticketing.Update(ticketing);

                var currentTicketing = _context.CurrentTicket.Where(x=> x.ticketingId == objGuid).FirstOrDefault();
                if (currentTicketing.typeOfTransaction != model["typeOfTransaction"].ToString())
                {
                    if (model["typeOfTransaction"].ToString() == "Trader truck")
                    {
                        TradersTruck tradersTruck = new TradersTruck
                        {
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
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
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
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
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
                            CommodityIn = ""
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
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
                            DriverName = currentTicketingIn.driverName
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
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
                            DriverName = currentTicketingIn.driverName
                        };

                        StallLease stallLease = _context.StallLease.Where(x => x.PlateNumber1 == currentTicketingIn.plateNumber || x.PlateNumber2 == currentTicketingIn.plateNumber).FirstOrDefault();

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
                    if (currentTicketingIn.typeOfTransaction == "Trader truck")
                    {
                        TradersTruck tradersTruck = new TradersTruck
                        {
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
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
                    else if (currentTicketingIn.typeOfTransaction == "Farmer truck")
                    {
                        FarmersTruck farmersTruck = new FarmersTruck
                        {
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
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
                    else if (currentTicketingIn.typeOfTransaction == "Short trip")
                    {
                        ShortTrip shortTrip = new ShortTrip
                        {
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
                            CommodityIn = "",
                            CommodityOut = ""
                        };
                        _context.ShortTrip.Update(shortTrip);
                        ShortTrip checkShortTrip = _context.ShortTrip.Where(x => x.ticketingId == objGuid).FirstOrDefault();
                        if (checkShortTrip == null)
                        {
                            return Json(new { success = false, message = "Edit limit is reached!" });
                        }
                    }
                    else if (currentTicketingIn.typeOfTransaction == "Gate pass")
                    {
                        StallLease stallLease = _context.StallLease.Where(x => x.PlateNumber1 == currentTicketingIn.plateNumber || x.PlateNumber2 == currentTicketingIn.plateNumber).FirstOrDefault();

                        GatePass gatePass = new GatePass
                        {
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
                            DriverName = currentTicketingIn.driverName
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
                            ticketingId = currentTicketingIn.ticketingId,
                            TimeIn = DateTime.Now,
                            PlateNumber = currentTicketingIn.plateNumber,
                            DriverName = currentTicketingIn.driverName
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
        public async Task<IActionResult> UpdateTicketOut([FromBody] JObject model)
        {
            Guid id = Guid.Empty;
            id = Guid.Parse(model["ticketingId"].ToString());

            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            var ticketingPrice = _context.TicketingPrice.FirstOrDefault();
            ticketing.timeOut = DateTime.Now;
            ticketing.Transaction = "Finished";
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

            if (ticketing.typeOfTransaction != "Farmer truck")
            {
                if (ticketing.plateNumber != model["plateNumber"].ToString() || ticketing.TypeOfEntry != model["TypeOfEntry"].ToString() || ticketing.typeOfCar != model["typeOfCar"].ToString() || ticketing.remarks != model["remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    var four = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Ticketing out/Finish",
                        EditedBy = info.FullName,
                        ControlNumber = ticketing.controlNumber.Value
                    };
                    if (ticketing.plateNumber != model["plateNumber"].ToString())
                    {
                        one = "Plate number = " + ticketing.plateNumber + " - " + model["plateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (ticketing.TypeOfEntry != model["TypeOfEntry"].ToString())
                    {
                        two = "Type of entry = " + ticketing.TypeOfEntry + " - " + model["TypeOfEntry"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (ticketing.typeOfCar != model["typeOfCar"].ToString())
                    {
                        three = "Type of car = " + ticketing.typeOfCar + " - " + model["typeOfCar"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (ticketing.remarks != model["remarks"].ToString())
                    {
                        four = "Remarks = " + ticketing.remarks + " - " + model["remarks"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    var datas = one + two + three + four;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }

                ticketing.plateNumber = model["plateNumber"].ToString();
                ticketing.typeOfCar = model["typeOfCar"].ToString();
                ticketing.TypeOfEntry = model["TypeOfEntry"].ToString();
                ticketing.PullOut = model["PullOut"].ToString();
                ticketing.remarks = model["remarks"].ToString();
            }

            else if (ticketing.typeOfTransaction == "Farmer truck")
            {
                if (ticketing.plateNumber != model["plateNumber"].ToString() || ticketing.typeOfCar != model["typeOfCar"].ToString() || ticketing.remarks != model["remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Ticketing out/Finish",
                        EditedBy = info.FullName,
                        ControlNumber = ticketing.controlNumber.Value
                    };
                    if (ticketing.plateNumber != model["plateNumber"].ToString())
                    {
                        one = "Plate number = " + ticketing.plateNumber + " - " + model["plateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (ticketing.typeOfCar != model["typeOfCar"].ToString())
                    {
                        two = "Type of car = " + ticketing.typeOfCar + " - " + model["typeOfCar"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (ticketing.remarks != model["remarks"].ToString())
                    {
                        three = "Remarks = " + ticketing.remarks + " - " + model["remarks"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    var datas = one + two + three;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }

                ticketing.plateNumber = model["plateNumber"].ToString();
                ticketing.typeOfCar = model["typeOfCar"].ToString();
                ticketing.PullOut = model["PullOut"].ToString();
                ticketing.remarks = model["remarks"].ToString();
            }

            if (ticketing.typeOfTransaction == "Trader truck")
            {
                TradersTruck tradersTruck = _context.TradersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                tradersTruck.TimeOut = ticketing.timeOut;
                if (ticketing.TypeOfEntry != "With transaction" && ticketing.TypeOfEntry != "Without transaction")
                {
                    return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                }
                else
                {
                if (ticketing.TypeOfEntry == "With transaction")
                    {
                    if (ticketing.PullOut == "No")
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
                                //ticketing.typeOfCar = "Overnight(1)";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended1NightPm)
                            {
                                //ticketing.typeOfCar = "Overnight(2)";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended2NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(3)";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended3NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(4)";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended4NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(5)";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended5NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(6)";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended6NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(7)";
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
                                    //ticketing.typeOfCar = "Overnight(1) with transaction";
                                    //ticketing.amount = 250;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                                }
                                else if (ticketing.timeOut <= extended2Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(2) with transaction";
                                    //ticketing.amount = 350;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                                }
                                else if (ticketing.timeOut <= extended3Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(3) with transaction";
                                    //ticketing.amount = 450;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                                }
                                else if (ticketing.timeOut <= extended4Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(4) with transaction";
                                    //ticketing.amount = 550;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                                }
                                else if (ticketing.timeOut <= extended5Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(5) with transaction";
                                    //ticketing.amount = 650;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                                }
                                else if (ticketing.timeOut <= extended6Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(6) with transaction";
                                    //ticketing.amount = 750;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                                }
                                else if (ticketing.timeOut <= extended7Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(7) with transaction";
                                    //ticketing.amount = 850;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                                }
                            }
                            else if (ticketing.timeOut <= am)
                            {
                                //ticketing.typeOfCar = "Overnight(1)";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended1Night)
                            {
                                //ticketing.typeOfCar = "Overnight(2)";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended2Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(3)";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended3Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(4)";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended4Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(5)";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended5Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(6)";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended6Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(7)";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                    }
                    else if (ticketing.PullOut == "Yes")
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
                                //ticketing.typeOfCar = "Overnight(1)";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended1NightPm)
                            {
                                //ticketing.typeOfCar = "Overnight(2)";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended2NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(3)";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended3NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(4)";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended4NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(5)";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended5NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(6)";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended6NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(7)";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                        else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                        {
                            if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                            {
                                var thirtyMinutes = ticketing.timeIn.Value.AddMinutes(30);
                                if (ticketing.timeOut <= thirtyMinutes)
                                {
                                    //ticketing.amount = 150;
                                    ticketing.typeOfCar = "Pull out";
                                    ticketing.amount = 20;
                                }
                                else if (ticketing.timeOut <= am)
                                {
                                    //ticketing.amount = 150;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction;
                                }
                                else if (ticketing.timeOut <= extended1Night)
                                {
                                    //ticketing.typeOfCar = "Overnight(1) with transaction";
                                    //ticketing.amount = 250;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                                }
                                else if (ticketing.timeOut <= extended2Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(2) with transaction";
                                    //ticketing.amount = 350;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                                }
                                else if (ticketing.timeOut <= extended3Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(3) with transaction";
                                    //ticketing.amount = 450;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                                }
                                else if (ticketing.timeOut <= extended4Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(4) with transaction";
                                    //ticketing.amount = 550;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                                }
                                else if (ticketing.timeOut <= extended5Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(5) with transaction";
                                    //ticketing.amount = 650;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                                }
                                else if (ticketing.timeOut <= extended6Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(6) with transaction";
                                    //ticketing.amount = 750;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                                }
                                else if (ticketing.timeOut <= extended7Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(7) with transaction";
                                    //ticketing.amount = 850;
                                    ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                                }
                            }
                            else if (ticketing.timeOut <= am)
                            {
                                //ticketing.typeOfCar = "Overnight(1)";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended1Night)
                            {
                                //ticketing.typeOfCar = "Overnight(2)";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended2Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(3)";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended3Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(4)";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended4Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(5)";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended5Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(6)";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended6Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(7)";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                    }
                }

                else if (ticketing.TypeOfEntry == "Without transaction")
                {
                    if (ticketing.PullOut == "No")
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
                            //ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 250;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended1NightPm)
                        {
                            //ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 350;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 2);
                        }
                        else if (ticketing.timeOut <= extended2NightsPm)
                        {
                            //ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 450;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 3);
                        }
                        else if (ticketing.timeOut <= extended3NightsPm)
                        {
                            //ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 550;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 4);
                        }
                        else if (ticketing.timeOut <= extended4NightsPm)
                        {
                            //ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 650;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 5);
                        }
                        else if (ticketing.timeOut <= extended5NightsPm)
                        {
                            //ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 750;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 6);
                        }
                        else if (ticketing.timeOut <= extended6NightsPm)
                        {
                            //ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 850;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 7);
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
                                //ticketing.typeOfCar = "Overnight(1) without transaction";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended2Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(2) without transaction";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended3Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(3) without transaction";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended4Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(4) without transaction";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended5Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(5) without transaction";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended6Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(6) without transaction";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended7Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(7) without transaction";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                        else if (ticketing.timeOut <= am)
                        {
                            //ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 250;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            //ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 350;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 2);
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 450;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 3);
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 550;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 4);
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 650;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 5);
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 750;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 6);
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 850;
                            ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 7);
                        }
                    }
                    }
                    else if (ticketing.PullOut == "Yes")
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
                                //ticketing.typeOfCar = "Overnight(1)";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended1NightPm)
                            {
                                //ticketing.typeOfCar = "Overnight(2)";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended2NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(3)";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended3NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(4)";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended4NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(5)";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended5NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(6)";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended6NightsPm)
                            {
                                //ticketing.typeOfCar = "Overnight(7)";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                        else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                        {
                            if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                            {
                                var thirtyMinutes = ticketing.timeIn.Value.AddMinutes(30);
                                if (ticketing.timeOut <= thirtyMinutes)
                                {
                                    //ticketing.amount = 100;
                                    ticketing.typeOfCar = "Pull out";
                                    ticketing.amount = 20;
                                }
                                else if (ticketing.timeOut <= am)
                                {
                                    //ticketing.amount = 100;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction;
                                }
                                else if (ticketing.timeOut <= extended1Night)
                                {
                                    //ticketing.typeOfCar = "Overnight(1) without transaction";
                                    //ticketing.amount = 250;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + ticketingPrice.PayParkingOvernight;
                                }
                                else if (ticketing.timeOut <= extended2Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(2) without transaction";
                                    //ticketing.amount = 350;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 2);
                                }
                                else if (ticketing.timeOut <= extended3Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(3) without transaction";
                                    //ticketing.amount = 450;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 3);
                                }
                                else if (ticketing.timeOut <= extended4Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(4) without transaction";
                                    //ticketing.amount = 550;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 4);
                                }
                                else if (ticketing.timeOut <= extended5Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(5) without transaction";
                                    //ticketing.amount = 650;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 5);
                                }
                                else if (ticketing.timeOut <= extended6Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(6) without transaction";
                                    //ticketing.amount = 750;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 6);
                                }
                                else if (ticketing.timeOut <= extended7Nights)
                                {
                                    //ticketing.typeOfCar = "Overnight(7) without transaction";
                                    //ticketing.amount = 850;
                                    ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 7);
                                }
                            }
                            else if (ticketing.timeOut <= am)
                            {
                                //ticketing.typeOfCar = "Overnight(1)";
                                //ticketing.amount = 250;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + ticketingPrice.PayParkingOvernight;
                            }
                            else if (ticketing.timeOut <= extended1Night)
                            {
                                //ticketing.typeOfCar = "Overnight(2)";
                                //ticketing.amount = 350;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 2);
                            }
                            else if (ticketing.timeOut <= extended2Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(3)";
                                //ticketing.amount = 450;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 3);
                            }
                            else if (ticketing.timeOut <= extended3Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(4)";
                                //ticketing.amount = 550;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 4);
                            }
                            else if (ticketing.timeOut <= extended4Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(5)";
                                //ticketing.amount = 650;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 5);
                            }
                            else if (ticketing.timeOut <= extended5Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(6)";
                                //ticketing.amount = 750;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 6);
                            }
                            else if (ticketing.timeOut <= extended6Nights)
                            {
                                //ticketing.typeOfCar = "Overnight(7)";
                                //ticketing.amount = 850;
                                ticketing.amount = ticketingPrice.TradersTruckWithoutTransaction + (ticketingPrice.PayParkingOvernight * 7);
                            }
                        }
                    }
                }
                }
                _context.TradersTruck.Update(tradersTruck);
            }
            else if (ticketing.typeOfTransaction == "Farmer truck")
            {
                FarmersTruck farmersTruck = _context.FarmersTruck.Where(x => x.ticketingId == id).FirstOrDefault();
                farmersTruck.TimeOut = ticketing.timeOut;
                if (ticketing.typeOfCar != "Single tire" && ticketing.typeOfCar != "Double tire")
                {
                    return Json(new { success = false, message = "Type of transaction does not match with type of car!" });
                }
                else
                {
                if (ticketing.PullOut == "No")
                {
                if (ticketing.typeOfCar == "Single tire")
                {
                    if (ticketing.timeOut <= extended1Night)
                    {
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 2);
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 3);
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 4);
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.FarmersTruckSingleTire + (ticketingPrice.PayParkingOvernight * 5);
                    }
                    else if (ticketing.timeOut <= extended7Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(7)";
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
                        //ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 250;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 350;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 2);
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 450;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 3);
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 550;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 4);
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 650;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 5);
                    }
                    else if (ticketing.timeOut <= extended7Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 750;
                        ticketing.amount = ticketingPrice.FarmersTruckDoubleTire + (ticketingPrice.PayParkingOvernight * 6);
                    }
                }
                    else if (ticketing.PullOut == "Yes")
                    {
                        var oneHour = ticketing.timeIn.Value.AddMinutes(60);
                        if (ticketing.timeOut <= oneHour)
                        {
                            ticketing.amount = 20;
                        }
                        else if (ticketing.timeOut > oneHour && ticketing.timeOut <= beforeMidnight)
                        {
                            ticketing.amount = 40;
                        }
                        else if (ticketing.timeOut <= afterMidnight)
                        {
                            ticketing.amount = 100;
                        }
                        else if (ticketing.timeOut <= afterMidnight.AddDays(1))
                        {
                            ticketing.amount = 200;
                        }
                        else if (ticketing.timeOut <= afterMidnight.AddDays(2))
                        {
                            ticketing.amount = 300;
                        }
                        else if (ticketing.timeOut <= afterMidnight.AddDays(3))
                        {
                            ticketing.amount = 400;
                        }
                        else if (ticketing.timeOut <= afterMidnight.AddDays(4))
                        {
                            ticketing.amount = 500;
                        }
                        else if (ticketing.timeOut <= afterMidnight.AddDays(5))
                        {
                            ticketing.amount = 600;
                        }
                        else if (ticketing.timeOut <= afterMidnight.AddDays(6))
                        {
                            ticketing.amount = 700;
                        }
                    }
                }
                }

                _context.FarmersTruck.Update(farmersTruck);
            }
            else if (ticketing.typeOfTransaction == "Short trip")
            {
                ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).OrderByDescending(x => x.Id).FirstOrDefault();
                shortTrip.TimeOut = ticketing.timeOut;

                var AM = ticketing.timeIn.Value;
                AM = new DateTime(AM.Year, AM.Month, AM.Day, 00, 00, 00);

                var pm = ticketing.timeIn.Value;
                pm = new DateTime(pm.Year, pm.Month, pm.Day, 12, 00, 00);
                if (ticketing.TypeOfEntry != "Delivery" && ticketing.TypeOfEntry != "Pick-up")
                {
                    return Json(new { success = false, message = "Type of transaction does not match with type of entry!" });
                }
                else
                {
                if (ticketing.timeIn >= pm && ticketing.timeIn <= midnight)
                {
                        if (midnight >= ticketing.timeOut)
                        {
                            //ticketing.amount = 20;
                            ticketing.amount = ticketingPrice.ShortTripPickUp;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            //ticketing.amount = 100;
                            //ticketing.typeOfCar = "Overnight(1)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.amount = 200;
                            //ticketing.typeOfCar = "Overnight(2)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.amount = 300;
                            //ticketing.typeOfCar = "Overnight(3)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.amount = 400;
                            //ticketing.typeOfCar = "Overnight(4)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.amount = 500;
                            //ticketing.typeOfCar = "Overnight(5)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.amount = 600;
                            //ticketing.typeOfCar = "Overnight(6)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended7Nights)
                        {
                            //ticketing.amount = 700;
                            //ticketing.typeOfCar = "Overnight(7)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }
                }
                else if (ticketing.timeIn >= AM && ticketing.timeIn <= pm)
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
                            //ticketing.typeOfCar = "Overnight(1)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.amount = 200;
                            //ticketing.typeOfCar = "Overnight(2)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.amount = 300;
                            //ticketing.typeOfCar = "Overnight(3)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.amount = 400;
                            //ticketing.typeOfCar = "Overnight(4)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.amount = 500;
                            //ticketing.typeOfCar = "Overnight(5)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.amount = 600;
                            //ticketing.typeOfCar = "Overnight(6)";
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended7Nights)
                        {
                            //ticketing.amount = 700;
                            //ticketing.typeOfCar = "Overnight(7)";
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
                        //ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1NightPm)
                    {
                        //ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(7)";
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
                            //ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 100;
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 200;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 300;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 400;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 500;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 600;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended6Nights.AddDays(1))
                        {
                            //ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 700;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }
                    }
                    else if (ticketing.timeOut <= am)
                    {
                        //ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        //ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(7)";
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
                        //ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1NightPm)
                    {
                        //ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6NightsPm)
                    {
                        //ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                    }
                }
                else if (ticketing.timeIn <= afterMidnight && ticketing.timeIn <= midnight)
                {
                    if (ticketing.timeIn <= beforeMidnight && ticketing.timeIn >= afterMidnight.AddDays(-1))
                    {
                        var fifteenMinutes = ticketing.timeIn.Value.AddMinutes(15);
                        if (ticketing.timeOut < fifteenMinutes)
                        {
                            ticketing.typeOfCar = "Less than 15 minutes";
                            //ticketing.amount = 40;
                            ticketing.amount = 0;
                        }
                        else if (ticketing.timeOut <= am)
                        {
                            //ticketing.typeOfCar = "Day time";
                            //ticketing.amount = 40;
                            ticketing.amount = ticketingPrice.PayParkingDaytime;
                        }
                        else if (ticketing.timeOut <= extended1Night)
                        {
                            //ticketing.typeOfCar = "Overnight(1)";
                            //ticketing.amount = 100;
                            ticketing.amount = ticketingPrice.PayParkingOvernight;
                        }
                        else if (ticketing.timeOut <= extended2Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(2)";
                            //ticketing.amount = 200;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                        }
                        else if (ticketing.timeOut <= extended3Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(3)";
                            //ticketing.amount = 300;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                        }
                        else if (ticketing.timeOut <= extended4Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(4)";
                            //ticketing.amount = 400;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                        }
                        else if (ticketing.timeOut <= extended5Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(5)";
                            //ticketing.amount = 500;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                        }
                        else if (ticketing.timeOut <= extended6Nights)
                        {
                            //ticketing.typeOfCar = "Overnight(6)";
                            //ticketing.amount = 600;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                        }
                        else if (ticketing.timeOut <= extended6Nights.AddDays(1))
                        {
                            //ticketing.typeOfCar = "Overnight(7)";
                            //ticketing.amount = 700;
                            ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                        }
                    }
                    else if (ticketing.timeOut <= am)
                    {
                        //ticketing.typeOfCar = "Overnight(1)";
                        //ticketing.amount = 100;
                        ticketing.amount = ticketingPrice.PayParkingOvernight;
                    }
                    else if (ticketing.timeOut <= extended1Night)
                    {
                        //ticketing.typeOfCar = "Overnight(2)";
                        //ticketing.amount = 200;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 2;
                    }
                    else if (ticketing.timeOut <= extended2Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(3)";
                        //ticketing.amount = 300;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 3;
                    }
                    else if (ticketing.timeOut <= extended3Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(4)";
                        //ticketing.amount = 400;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 4;
                    }
                    else if (ticketing.timeOut <= extended4Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(5)";
                        //ticketing.amount = 500;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 5;
                    }
                    else if (ticketing.timeOut <= extended5Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(6)";
                        //ticketing.amount = 600;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 6;
                    }
                    else if (ticketing.timeOut <= extended6Nights)
                    {
                        //ticketing.typeOfCar = "Overnight(7)";
                        //ticketing.amount = 700;
                        ticketing.amount = ticketingPrice.PayParkingOvernight * 7;
                    }
                }

                _context.PayParking.Update(payParking);
            }

            var currentParkingNumber = ticketing.parkingNumber;
            ParkingNumbers parkingNumber = _context.ParkingNumbers.Where(x => x.Name == currentParkingNumber).FirstOrDefault();
            parkingNumber.Selected = false;
            _context.ParkingNumbers.Update(parkingNumber);

            ticketing.receivingClerk = info.FullName;
            var amount = ticketing.amount;

            var credit = _context.Ticketing.Where(x => x.plateNumber == model["plateNumber"].ToString()).OrderByDescending(x => x.timeIn).Select(x => x.TotalCredit).FirstOrDefault();
            ticketing.TypeOfPayment = model["TypeOfPayment"].ToString();
            if (ticketing.TypeOfPayment == "Cash")
            {
                ticketing.amount = ticketing.amount;
                ticketing.TotalCredit = credit;

                Total total = new Total
                {
                    ticketingId = ticketing.ticketingId,
                    origin = "Ticketing cash",
                    date = DateTime.Now,
                    amount = ticketing.amount.Value
                };
                _context.Total.Add(total);
            }
            else if (ticketing.TypeOfPayment == "Credit")
            {
                ticketing.amount = ticketing.amount;
                ticketing.TotalCredit = credit + ticketing.amount.Value;
            }

            var timeIn = ticketing.timeIn.Value;
            var currentDate = DateTime.Now;

            var current = DateTime.Now;
            TimeSpan solve = current - ticketing.timeIn.Value;
            int seconds = (int)solve.TotalSeconds;
            int mins = seconds /60;
            int hrs = mins / 60;
            int days = hrs / 24;

            hrs = hrs - (days * 24);
            mins = mins - (days * 24 * 60) - (hrs * 60);
            seconds = seconds - (days * 24 * 60 * 60) - (hrs * 60 * 60) - (hrs * 60);

            //TimeSpan seconds = Math.Abs(currentDate - timeIn);
            //var minutes = Math.floor(seconds / 60);
            //var hours = Math.floor(minutes / 60);
            //var days = Math.floor(hours / 24);

            //hours = hours - (days * 24);
            //minutes = minutes - (days * 24 * 60) - (hours * 60);
            //seconds = seconds - (days * 24 * 60 * 60) - (hours * 60 * 60) - (minutes * 60);

            ticketing.TimeSpan = days + "days " + hrs + "hrs " + mins + "mins";

            //int seconds = (currentDate - timeIn) /1000;
            //int sec = (int)seconds.tota;

            //var minutes = seconds / 60;
            //var hours = minutes / 60;
            //var days = hours / 24;


            //hours = hours - (days * 24);
            //minutes = minutes - (days * 24 * 60) - (hours * 60);
            //seconds = seconds - (days * 24 * 60 * 60) - (hours * 60 * 60) - (minutes * 60);

            //ticketing.TimeSpan = days + "days" + hours + "hrs" + minutes + "mins";

            _context.Ticketing.Update(ticketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Your payment amount is " + amount });
        }

        // POST: api/Ticketing/PostDebit
        [HttpPost("PostDebit")]
        public async Task<IActionResult> PostDebit([FromBody] JObject model)
        {
            Guid objGuid = Guid.Empty;
            var getLastControlNumber = _context.Ticketing.OrderByDescending(x => x.controlNumber).Select(x => x.controlNumber).FirstOrDefault();
            //int? controlNumber = getLastControlNumber + 1;
            var info = await _userManager.GetUserAsync(User);
            objGuid = Guid.Parse(model["ticketingId"].ToString());

            
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == objGuid).OrderByDescending(x => x.timeIn).FirstOrDefault();
            ticketing.DebitAmount = Convert.ToInt32(model["DebitAmount"].ToString());
            ticketing.TotalCredit = ticketing.TotalCredit - ticketing.DebitAmount;
            ticketing.DebitReceiver = info.FullName;
            _context.Ticketing.Update(ticketing);

            Total total = new Total
            {
                ticketingId = ticketing.ticketingId,
                origin = "Ticketing debit",
                date = DateTime.Now,
                amount = ticketing.DebitAmount
            };

            _context.Total.Add(total);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/ExtendGatePass
        [HttpPost("ExtendGatePass")]
        public async Task<IActionResult> ExtendGatePass(Guid id)
        {
            var info = await _userManager.GetUserAsync(User);
            //Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            StallLease stallLease = _context.StallLease.Where(x => x.ticketingId == id).FirstOrDefault();

            //ticketing.timeOut = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            DateTime endDate = new DateTime(currentDate.Year, 12, 31);

            stallLease.EndDate = endDate;
            //gatePass.EndDate = gatePass.EndDate.Value.AddYears(1);
            stallLease.Amount = stallLease.Amount + 500;

            Total total = new Total
            {
                ticketingId = stallLease.ticketingId,
                origin = "Gate pass",
                date = DateTime.Now,
                amount = 500
            };

            _context.Total.Add(total);
            _context.StallLease.Update(stallLease);

            EditedDatas editedDatas = new EditedDatas
            {
                DateEdited = DateTime.Now,
                Origin = "Avail gate pass/Extend",
                EditedBy = info.FullName,
                ControlNumber = stallLease.ControlNumber.Value
            };
            editedDatas.EditedData = "Status = Expired - Valid;";
            _context.EditedDatas.Add(editedDatas);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Ticketing/ExtendGatePass
        [HttpPost("AddCount")]
        public async Task<IActionResult> AddCount(Guid id)
        {
            var info = await _userManager.GetUserAsync(User);
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

            EditedDatas editedDatas = new EditedDatas
            {
                DateEdited = DateTime.Now,
                Origin = "Ticketing in/Count",
                EditedBy = info.FullName,
                ControlNumber = ticketing.controlNumber.Value
            };
            var origCount = ticketing.count.Value - 1;
            editedDatas.EditedData = "Count = " + origCount + " - " + ticketing.count.Value + ";";
            _context.EditedDatas.Add(editedDatas);

            var currentShortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).OrderByDescending(x => x.DateInspectedIn).FirstOrDefault();
            currentShortTrip.TimeOut = DateTime.Now;
            _context.ShortTrip.Update(currentShortTrip);

            ShortTrip shortTrip = new ShortTrip 
            { 
                TimeIn = DateTime.Now,
                PlateNumber = ticketing.plateNumber,
                ParkingNumber = ticketing.parkingNumber,
                ticketingId = ticketing.ticketingId
            };
            shortTrip.CommodityIn = "";
            shortTrip.CommodityOut = "";
            _context.ShortTrip.Add(shortTrip);

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
            var getLastControlNumber = _context.StallLease.OrderByDescending(x => x.ControlNumber).Select(x => x.ControlNumber).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);

            StallLease stallLease = new StallLease
            {
                StartDate = startDate,
                EndDate = endDate,
                DriverName = model["DriverName"].ToString(),
                //LastName = model["LastName"].ToString(),
                PlateNumber1 = model["PlateNumber1"].ToString(),
                PlateNumber2 = model["PlateNumber2"].ToString(),
                //Status = Convert.ToInt32(model["Status"].ToString()),
                ContactNumber = model["ContactNumber"].ToString(),
                StallNumber = model["StallNumber"].ToString(),
                //IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Remarks = model["Remarks"].ToString(),
                Amount = amount
            };
            if (getLastControlNumber == null)
            {
                stallLease.ControlNumber = 1;
            }
            else
            {
                stallLease.ControlNumber = getLastControlNumber.Value + 1;
            }

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
                StallLease currentStallLease = _context.StallLease.Where(x => x.ticketingId == id).FirstOrDefault();
                if (currentStallLease.DriverName != model["DriverName"].ToString() || currentStallLease.ContactNumber != model["ContactNumber"].ToString() || currentStallLease.PlateNumber1 != model["PlateNumber1"].ToString() || currentStallLease.PlateNumber2 != model["PlateNumber2"].ToString() || currentStallLease.StallNumber != model["StallNumber"].ToString() || currentStallLease.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    var four = "";
                    var five = "";
                    var six = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Avail gate pass",
                        EditedBy = info.FullName,
                        ControlNumber = currentStallLease.ControlNumber.Value
                    };
                    if (currentStallLease.DriverName != model["DriverName"].ToString())
                    {
                        one = "Drivers name = " + currentStallLease.DriverName + " - " + model["DriverName"].ToString() + "; ";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentStallLease.ContactNumber != model["ContactNumber"].ToString())
                    {
                        two = " Contact number = " + currentStallLease.ContactNumber + " - " + model["ContactNumber"].ToString() + "; ";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentStallLease.PlateNumber1 != model["PlateNumber1"].ToString())
                    {
                        three = " Plate number 1 = " + currentStallLease.PlateNumber1 + " - " + model["PlateNumber1"].ToString() + "; ";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentStallLease.PlateNumber2 != model["PlateNumber2"].ToString())
                    {
                        four = " Plate number 2 = " + currentStallLease.PlateNumber2 + " - " + model["PlateNumber2"].ToString() + "; ";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentStallLease.StallNumber != model["StallNumber"].ToString())
                    {
                        five = " Stall number = " + currentStallLease.StallNumber + " - " + model["StallNumber"].ToString() + "; ";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentStallLease.Remarks != model["Remarks"].ToString())
                    {
                        five = " Remarks = " + currentStallLease.Remarks + " - " + model["Remarks"].ToString() + "; ";
                    }
                    else
                    {
                        five = "";
                    }
                    var datas = one + two + three + four + five + six;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                    //currentStallLease.DriverName = model["DriverName"].ToString();
                currentStallLease.PlateNumber1 = model["PlateNumber1"].ToString();
                currentStallLease.PlateNumber2 = model["PlateNumber2"].ToString();
                currentStallLease.DriverName = model["DriverName"].ToString();
                currentStallLease.ContactNumber = model["ContactNumber"].ToString();
                currentStallLease.StallNumber = model["StallNumber"].ToString();
                currentStallLease.Remarks = model["Remarks"].ToString();
                _context.StallLease.Update(currentStallLease);

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

        //// GET: api/Ticketing/GetParkingNumber
        //[HttpGet("GetParkingNumber")]
        //public IActionResult GetParkingNumber([FromRoute]Guid organizationId)
        //{
        //    var listParking = _context.ParkingNumbers.Where(x => x.Selected == false).ToList();

        //    return Json(new { data = listParking });
        //}

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
                var shortTripCheck = _context.ShortTrip.Where(x => x.ticketingId == id).Count();
                if (shortTripCheck > 1)
                {
                    return Json(new { success = false, message = "Cannot delete multiple entries!" });
                }
                ShortTrip shortTrip = _context.ShortTrip.Where(x => x.ticketingId == id).FirstOrDefault();
                _context.Remove(shortTrip);
            }
            else if (ticketing.typeOfTransaction == "Pay parking")
            {
                PayParking payParking = _context.PayParking.Where(x => x.ticketingId == id).FirstOrDefault();
                _context.Remove(payParking);
            }

            ParkingNumbers parkingNumbers = _context.ParkingNumbers.Where(x => x.Name == ticketing.parkingNumber).FirstOrDefault();
            parkingNumbers.Selected = false;
            _context.ParkingNumbers.Update(parkingNumbers);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = ticketing.plateNumber,
                Origin = "Ticketing",
                Name = ticketing.driverName,
                DeletedBy = info.FullName,
                Remarks = ticketing.remarks,
                Amount = ticketing.amount
            };
            _context.DeletedDatas.Add(deleted);

            DeletedDatas deleted2 = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = ticketing.plateNumber,
                Origin = ticketing.typeOfTransaction,
                Name = ticketing.driverName,
                DeletedBy = info.FullName,
                Remarks = ticketing.remarks
            };
            _context.DeletedDatas.Add(deleted2);

            Total total = _context.Total.Where(x => x.ticketingId == id).FirstOrDefault();
            if (total != null)
            {
                _context.Remove(total);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Ticketing/DeleteGatePass
        [HttpDelete("DeleteGatePass/{id}")]
        public async Task<IActionResult> DeleteGatePass([FromRoute] Guid id)
        {
            StallLease stallLease = _context.StallLease.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(stallLease);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
               DateDeleted = DateTime.Now,
               PlateNumber = stallLease.PlateNumber1 + " " + stallLease.PlateNumber2,
               Origin = "Stall leasers",
               Name = stallLease.DriverName,
               DeletedBy = info.FullName,
               Remarks = stallLease.Remarks,
               Amount = stallLease.Amount
            };
            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }


}