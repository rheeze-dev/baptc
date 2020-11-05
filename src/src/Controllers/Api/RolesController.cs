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
    [Route("api/Roles")]
    //[Authorize]
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RolesController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/Roles
        [HttpGet("{organizationId}")]
        public IActionResult GetRoles([FromRoute]Guid organizationId)
        {
            var roles = _context.Role.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = roles });
        }

        // GET: api/Roles
        [HttpGet("GetParkingNumbers")]
        public IActionResult GetParkingNumbers([FromRoute]Guid organizationId)
        {
            var parkingNumbers = _context.ParkingNumbers.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = parkingNumbers });
        }

        // GET: api/Roles/GetAddresses
        [HttpGet("GetAddresses")]
        public IActionResult GetAddresses([FromRoute]Guid organizationId)
        {
            var addresses = _context.Addresses.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = addresses });
        }

        // GET: api/Roles/GetCommodities
        [HttpGet("GetCommodities")]
        public IActionResult GetCommodities([FromRoute]Guid organizationId)
        {
            var commodties = _context.Commodities.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = commodties });
        }

        // GET: api/Roles
        [HttpGet("GetDeletedDatas")]
        public IActionResult GetDeletedDatas([FromRoute]Guid organizationId)
        {
            var deleted = _context.DeletedDatas.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = deleted });
        }

        // GET: api/Roles
        [HttpGet("GetEditedDatas")]
        public IActionResult GetEditedDatas([FromRoute]Guid organizationId)
        {
            var edited = _context.EditedDatas.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = edited });
        }

        // POST: api/Roles/PostTicketingPrice
        [HttpPost("PostTicketingPrice")]
        public async Task<IActionResult> PostTicketingPrice([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            var info = await _userManager.GetUserAsync(User);
           
            if (id == 0)
            {
                TicketingPrice ticketingPrice = new TicketingPrice
                {
                    DateAdded = DateTime.Now,
                    TradersTruckWithTransaction = Convert.ToInt32(model["TradersTruckWithTransaction"].ToString()),
                    TradersTruckWithoutTransaction = Convert.ToInt32(model["TradersTruckWithoutTransaction"].ToString()),
                    FarmersTruckSingleTire = Convert.ToInt32(model["FarmersTruckSingleTire"].ToString()),
                    FarmersTruckDoubleTire = Convert.ToInt32(model["FarmersTruckDoubleTire"].ToString()),
                    ShortTripPickUp = Convert.ToInt32(model["ShortTripPickUp"].ToString()),
                    ShortTripDelivery = Convert.ToInt32(model["ShortTripDelivery"].ToString()),
                    PayParkingDaytime = Convert.ToInt32(model["PayParkingDaytime"].ToString()),
                    PayParkingOvernight = Convert.ToInt32(model["PayParkingOvernight"].ToString()),
                    StallLeasePrice = Convert.ToInt32(model["StallLeasePrice"].ToString()),
                    Editor = info.FullName
                };
                _context.TicketingPrice.Add(ticketingPrice);
            }
            else
            {
                var currentTicketPrice = _context.TicketingPrice.Where(x => x.Id == id).FirstOrDefault();
                if (currentTicketPrice.TradersTruckWithTransaction != Convert.ToInt32(model["TradersTruckWithTransaction"].ToString()) || currentTicketPrice.TradersTruckWithoutTransaction != Convert.ToInt32(model["TradersTruckWithoutTransaction"].ToString()) || currentTicketPrice.FarmersTruckSingleTire != Convert.ToInt32(model["FarmersTruckSingleTire"].ToString()) || currentTicketPrice.FarmersTruckDoubleTire != Convert.ToInt32(model["FarmersTruckDoubleTire"].ToString()) || currentTicketPrice.ShortTripDelivery != Convert.ToInt32(model["ShortTripDelivery"].ToString()) || currentTicketPrice.ShortTripPickUp != Convert.ToInt32(model["ShortTripPickUp"].ToString()) || currentTicketPrice.PayParkingDaytime != Convert.ToInt32(model["PayParkingDaytime"].ToString()) || currentTicketPrice.PayParkingOvernight != Convert.ToInt32(model["PayParkingOvernight"].ToString()) || currentTicketPrice.StallLeasePrice != Convert.ToInt32(model["StallLeasePrice"].ToString()))
                {
                    var tradersWith = "";
                    var tradersWithOut = "";
                    var farmersSingle = "";
                    var farmersDouble = "";
                    var shortTripPick = "";
                    var shortTripDelivery = "";
                    var payparkingDay = "";
                    var payparkingOvernight = "";
                    var gatepass = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Ticketing price",
                        EditedBy = info.FullName,
                        ControlNumber = currentTicketPrice.Id
                    };
                    if (currentTicketPrice.TradersTruckWithTransaction != Convert.ToInt32(model["TradersTruckWithTransaction"].ToString()))
                    {
                        tradersWith = "Traders with transaction = " + currentTicketPrice.TradersTruckWithTransaction + " - " + Convert.ToInt32(model["TradersTruckWithTransaction"].ToString()) + "; ";
                    }
                    else
                    {
                        tradersWith = "";
                    }
                    if (currentTicketPrice.TradersTruckWithoutTransaction != Convert.ToInt32(model["TradersTruckWithoutTransaction"].ToString()))
                    {
                        tradersWithOut = " Traders without transaction = " + currentTicketPrice.TradersTruckWithoutTransaction + " - " + Convert.ToInt32(model["TradersTruckWithoutTransaction"].ToString()) + "; ";
                    }
                    else
                    {
                        tradersWithOut = "";
                    }
                    if (currentTicketPrice.FarmersTruckSingleTire != Convert.ToInt32(model["FarmersTruckSingleTire"].ToString()))
                    {
                        farmersSingle = " Farmers single tire = " + currentTicketPrice.FarmersTruckSingleTire + " - " + Convert.ToInt32(model["FarmersTruckSingleTire"].ToString()) + "; ";
                    }
                    else
                    {
                        farmersSingle = "";
                    }
                    if (currentTicketPrice.FarmersTruckDoubleTire != Convert.ToInt32(model["FarmersTruckDoubleTire"].ToString()))
                    {
                        farmersDouble = " Farmers double tire = " + currentTicketPrice.FarmersTruckDoubleTire + " - " + Convert.ToInt32(model["FarmersTruckDoubleTire"].ToString()) + "; ";
                    }
                    else
                    {
                        farmersDouble = "";
                    }
                    if (currentTicketPrice.ShortTripPickUp != Convert.ToInt32(model["ShortTripPickUp"].ToString()))
                    {
                        shortTripPick = " Short trip pick-up = " + currentTicketPrice.ShortTripPickUp + " - " + Convert.ToInt32(model["ShortTripPickUp"].ToString()) + "; ";
                    }
                    else
                    {
                        shortTripPick = "";
                    }
                    if (currentTicketPrice.ShortTripDelivery != Convert.ToInt32(model["ShortTripDelivery"].ToString()))
                    {
                        shortTripDelivery = " Short trip delivery = " + currentTicketPrice.ShortTripDelivery + " - " + Convert.ToInt32(model["ShortTripDelivery"].ToString()) + "; ";
                    }
                    else
                    {
                        shortTripDelivery = "";
                    }
                    if (currentTicketPrice.PayParkingDaytime != Convert.ToInt32(model["PayParkingDaytime"].ToString()))
                    {
                        payparkingDay = " Pay parking daytime = " + currentTicketPrice.PayParkingDaytime + " - " + Convert.ToInt32(model["PayParkingDaytime"].ToString()) + "; ";
                    }
                    else
                    {
                        payparkingDay = "";
                    }
                    if (currentTicketPrice.PayParkingOvernight != Convert.ToInt32(model["PayParkingOvernight"].ToString()))
                    {
                        payparkingOvernight = " Pay parking overnight = " + currentTicketPrice.PayParkingOvernight + " - " + Convert.ToInt32(model["PayParkingOvernight"].ToString()) + "; ";
                    }
                    else
                    {
                        payparkingOvernight = "";
                    }
                    if (currentTicketPrice.StallLeasePrice != Convert.ToInt32(model["StallLeasePrice"].ToString()))
                    {
                        gatepass = " Gate pass = " + currentTicketPrice.StallLeasePrice + " - " + Convert.ToInt32(model["StallLeasePrice"].ToString()) + "; ";
                    }
                    else
                    {
                        gatepass = "";
                    }
                    var datas = tradersWith + tradersWithOut + farmersSingle + farmersDouble + shortTripPick + shortTripDelivery + payparkingDay + payparkingOvernight + gatepass;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                currentTicketPrice.TradersTruckWithTransaction = Convert.ToInt32(model["TradersTruckWithTransaction"].ToString());
                currentTicketPrice.TradersTruckWithoutTransaction = Convert.ToInt32(model["TradersTruckWithoutTransaction"].ToString());
                currentTicketPrice.FarmersTruckSingleTire = Convert.ToInt32(model["FarmersTruckSingleTire"].ToString());
                currentTicketPrice.FarmersTruckDoubleTire = Convert.ToInt32(model["FarmersTruckDoubleTire"].ToString());
                currentTicketPrice.ShortTripPickUp = Convert.ToInt32(model["ShortTripPickUp"].ToString());
                currentTicketPrice.ShortTripDelivery = Convert.ToInt32(model["ShortTripDelivery"].ToString());
                currentTicketPrice.PayParkingDaytime = Convert.ToInt32(model["PayParkingDaytime"].ToString());
                currentTicketPrice.PayParkingOvernight = Convert.ToInt32(model["PayParkingOvernight"].ToString());
                currentTicketPrice.StallLeasePrice = Convert.ToInt32(model["StallLeasePrice"].ToString());
                currentTicketPrice.Editor = info.FullName;
                _context.TicketingPrice.Update(currentTicketPrice);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostRoles
        [HttpPost]
        public async Task<IActionResult> PostRoles([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            
            if (id == 0)
            {
                Roles roles = new Roles
                {
                    DateAdded = DateTime.Now,
                    FullName = model["FullName"].ToString(),
                    Remarks = model["Remarks"].ToString(),
                    ShortName = model["ShortName"].ToString(),
                    Name = model["ShortName"].ToString(),
                    Modifier = info.FullName
                };
                _context.Role.Add(roles);
            }
            else
            {
                var currentRoles = _context.Role.Where(x => x.Id == id).FirstOrDefault();
                if (currentRoles.FullName != model["FullName"].ToString() || currentRoles.ShortName != model["ShortName"].ToString() || currentRoles.Name != model["ShortName"].ToString() || currentRoles.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    //var four = "";
                    //var five = "";
                    //var six = "";
                    //var seven = "";
                    //var eight = "";
                    //var nine = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Addresses",
                        EditedBy = info.FullName,
                        ControlNumber = currentRoles.Id
                    };
                    editedDatas.Remarks = model["Remarks"].ToString();
                    if (currentRoles.FullName != model["FullName"].ToString())
                    {
                        one = "Full name = " + currentRoles.FullName + " - " + model["FullName"].ToString() + "; ";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentRoles.ShortName != model["ShortName"].ToString())
                    {
                        two = " Short name = " + currentRoles.ShortName + " - " + model["ShortName"].ToString() + "; ";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentRoles.Remarks != model["Remarks"].ToString())
                    {
                        three = " Remarks = " + currentRoles.Remarks + " - " + model["Remarks"].ToString() + "; ";
                    }
                    else
                    {
                        three = "";
                    }
                    var datas = one + two + three;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                currentRoles.FullName = model["FullName"].ToString();
                currentRoles.Remarks = model["Remarks"].ToString();
                currentRoles.ShortName = model["ShortName"].ToString();
                currentRoles.Name = model["ShortName"].ToString();
                currentRoles.Modifier = info.FullName;
                _context.Role.Update(currentRoles);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostParkingNumbers
        [HttpPost("PostParkingNumbers")]
        public async Task<IActionResult> PostParkingNumbers([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            
            if (id == 0)
            {
                ParkingNumbers parkingNumbers = new ParkingNumbers
                {
                    DateAdded = DateTime.Now,
                    //FullName = model["FullName"].ToString(),
                    Remarks = model["Remarks"].ToString(),
                    Name = model["Name"].ToString(),
                    Modifier = info.FullName
                };
                _context.ParkingNumbers.Add(parkingNumbers);
            }
            else
            {
                var currentParking = _context.ParkingNumbers.Where(x => x.Id == id).FirstOrDefault();
                if (currentParking.Remarks != model["Remarks"].ToString() || currentParking.Name != model["Name"].ToString())
                {
                    var one = "";
                    var two = "";
                    //var four = "";
                    //var five = "";
                    //var six = "";
                    //var seven = "";
                    //var eight = "";
                    //var nine = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Parking",
                        EditedBy = info.FullName,
                        ControlNumber = currentParking.Id
                    };
                    editedDatas.Remarks = model["Remarks"].ToString();
                    if (currentParking.Name != model["Name"].ToString())
                    {
                        one = " Short name = " + currentParking.Name + " - " + model["Name"].ToString() + "; ";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentParking.Remarks != model["Remarks"].ToString())
                    {
                        two = " Remarks = " + currentParking.Remarks + " - " + model["Remarks"].ToString() + "; ";
                    }
                    else
                    {
                        two = "";
                    }
                    var datas = one + two;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                currentParking.Remarks = model["Remarks"].ToString();
                currentParking.Name = model["Name"].ToString();
                currentParking.Modifier = info.FullName;
                _context.ParkingNumbers.Update(currentParking);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostAddresses
        [HttpPost("PostAddresses")]
        public async Task<IActionResult> PostAddresses([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            
            if (id == 0)
            {
                Addresses addresses = new Addresses
                {
                    Barangay = model["Barangay"].ToString(),
                    Municipality = model["Municipality"].ToString(),
                    Province = model["Province"].ToString(),
                    Remarks = model["Remarks"].ToString(),
                    Date = DateTime.Now,
                    Modifier = info.FullName
                };
                _context.Addresses.Add(addresses);
            }
            else
            {
                var currentAddresses = _context.Addresses.Where(x => x.Id == id).FirstOrDefault();
                if (currentAddresses.Barangay != model["Barangay"].ToString() || currentAddresses.Municipality != model["Municipality"].ToString() || currentAddresses.Province != model["Province"].ToString() || currentAddresses.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    var three = "";
                    var four = "";
                    //var five = "";
                    //var six = "";
                    //var seven = "";
                    //var eight = "";
                    //var nine = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Addresses",
                        EditedBy = info.FullName,
                        ControlNumber = currentAddresses.Id
                    };
                    editedDatas.Remarks = model["Remarks"].ToString();
                    if (currentAddresses.Barangay != model["Barangay"].ToString())
                    {
                        one = "Barangay = " + currentAddresses.Barangay + " - " + model["Barangay"].ToString() + "; ";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentAddresses.Municipality != model["Municipality"].ToString())
                    {
                        two = " Municipality = " + currentAddresses.Municipality + " - " + model["Municipality"].ToString() + "; ";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentAddresses.Province != model["Province"].ToString())
                    {
                        three = " Province = " + currentAddresses.Province + " - " + model["Province"].ToString() + "; ";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentAddresses.Remarks != model["Remarks"].ToString())
                    {
                        four = " Remarks = " + currentAddresses.Remarks + " - " + model["Remarks"].ToString() + "; ";
                    }
                    else
                    {
                        four = "";
                    }
                    var datas = one + two + three + four;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                currentAddresses.Barangay = model["Barangay"].ToString();
                currentAddresses.Municipality = model["Municipality"].ToString();
                currentAddresses.Province = model["Province"].ToString();
                currentAddresses.Remarks = model["Remarks"].ToString();
                currentAddresses.Modifier = info.FullName;
                _context.Addresses.Update(currentAddresses);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostCommodities
        [HttpPost("PostCommodities")]
        public async Task<IActionResult> PostCommodities([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            
            if (id == 0)
            {
                Commodities commodities = new Commodities
                {
                    Commodity = model["Commodity"].ToString(),
                    Remarks = model["Remarks"].ToString(),
                    Date = DateTime.Now,
                    Modifier = info.FullName
                };
                _context.Commodities.Add(commodities);
            }
            else
            {
                var currentCommodities = _context.Commodities.Where(x => x.Id == id).FirstOrDefault();
                if (currentCommodities.Commodity != model["Commodity"].ToString() || currentCommodities.Remarks != model["Remarks"].ToString())
                {
                    var one = "";
                    var two = "";
                    //var three = "";
                    //var four = "";
                    //var five = "";
                    //var six = "";
                    //var seven = "";
                    //var eight = "";
                    //var nine = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Commodities",
                        EditedBy = info.FullName,
                        ControlNumber = currentCommodities.Id
                    };
                    editedDatas.Remarks = model["Remarks"].ToString();
                    if (currentCommodities.Commodity != model["Commodity"].ToString())
                    {
                        one = "Commodity = " + currentCommodities.Commodity + " - " + model["Commodity"].ToString() + "; ";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentCommodities.Remarks != model["Remarks"].ToString())
                    {
                        two = " Remarks = " + currentCommodities.Remarks + " - " + model["Remarks"].ToString() + "; ";
                    }
                    else
                    {
                        two = "";
                    }
                    var datas = one + two;
                    editedDatas.EditedData = datas;
                    _context.EditedDatas.Add(editedDatas);
                }
                currentCommodities.Commodity = model["Commodity"].ToString();
                currentCommodities.Remarks = model["Remarks"].ToString();
                currentCommodities.Modifier = info.FullName;
                _context.Commodities.Update(currentCommodities);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Roles
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoles([FromRoute] int id)
        {
            var info = await _userManager.GetUserAsync(User);
            Roles roles = _context.Role.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(roles);

            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Roles",
                Name = roles.FullName,
                DeletedBy = info.FullName,
                Remarks = roles.Remarks
            };

            _context.DeletedDatas.Add(deleted);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles/DeleteParkingNumbers
        [HttpDelete("DeleteParkingNumbers/{id}")]
        public async Task<IActionResult> DeleteParkingNumbers([FromRoute] int id)
        {
            var info = await _userManager.GetUserAsync(User);
            ParkingNumbers parking = _context.ParkingNumbers.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(parking);

            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Parking",
                Name = parking.Name,
                DeletedBy = info.FullName,
                Remarks = parking.Remarks
            };

            _context.DeletedDatas.Add(deleted);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles/DeleteCommodities
        [HttpDelete("DeleteCommodities/{id}")]
        public async Task<IActionResult> DeleteCommodities([FromRoute] int id)
        {
            var info = await _userManager.GetUserAsync(User);
            Commodities commodities = _context.Commodities.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(commodities);

            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Commodities",
                Name = commodities.Commodity,
                DeletedBy = info.FullName,
                Remarks = commodities.Remarks
            };

            _context.DeletedDatas.Add(deleted);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles/DeleteAddresses
        [HttpDelete("DeleteAddresses/{id}")]
        public async Task<IActionResult> DeleteAddresses([FromRoute] int id)
        {
            var info = await _userManager.GetUserAsync(User);
            Addresses addresses = _context.Addresses.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(addresses);

            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Addresses",
                Name = addresses.Barangay,
                DeletedBy = info.FullName,
                Remarks = addresses.Remarks
            };

            _context.DeletedDatas.Add(deleted);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles
        [HttpDelete("UserRoles/{id}")]
        public async Task<IActionResult> DeleteUserRoles([FromRoute] int id)
        {
            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            _context.Remove(applicationUser);
            var info = await _userManager.GetUserAsync(User);

            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Users",
                Name = applicationUser.Email,
                DeletedBy = info.FullName,
                Remarks = ""
            };
            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // GET: api/Roles/GetUsers
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            List<ApplicationUser> listUser = _context.ApplicationUser.OrderByDescending(x => x.UserId).ToList();
            return Json(new { data = listUser });
        }

        // GET: api/Roles/GetUsers
        [HttpGet("GetTicketingPrice")]
        public IActionResult GetTicketingPrice()
        {
            var ticketingPrice = _context.TicketingPrice.ToList();
            return Json(new { data = ticketingPrice });
        }

        //[HttpPost("ConfigUserRoles")]
        public IActionResult ConfigUserRole(int userId)
        {
            ApplicationUser applicationUser = _context.ApplicationUser.Where(user => user.UserId == userId).FirstOrDefault();
            return Json(new { data = applicationUser });
        }

        // POST: api/Roles/PostUserRole
        [HttpPost("PostUserRole")]
        public async Task<IActionResult> PostUserRole(ApplicationUser applicationUser, UserRole userRole)
        {
            int id = applicationUser.UserId;
            int userRoleId = userRole.Id;
            var info = await _userManager.GetUserAsync(User);
             
            ApplicationUser updateApplicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
                {
                if (applicationUser.RoleId != updateApplicationUser.RoleId || applicationUser.Modules != updateApplicationUser.Modules)
                {
                    var role = "";
                    var module = "";
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Users/Config",
                        EditedBy = info.FullName,
                        ControlNumber = updateApplicationUser.UserId
                    };
                    role = "Role = " + updateApplicationUser.RoleId + " - " + applicationUser.RoleId + ";";
                    module = "Modules = " + updateApplicationUser.Modules + " - " + applicationUser.Modules + ";";
                    var config = role + module;
                    editedDatas.EditedData = config;
                    editedDatas.Remarks = userRole.Remarks;
                    _context.EditedDatas.Add(editedDatas);
                }
                updateApplicationUser.RoleId = applicationUser.RoleId;
                    updateApplicationUser.Modules = applicationUser.Modules;
                    updateApplicationUser.DateModified = DateTime.Now;
                    updateApplicationUser.Modifier = info.FullName;
                if (applicationUser.RoleId == null)
                {
                    return Json(new { success = false, message = "Role field cannot be empty!" });
                }
                else if (applicationUser.RoleId.Contains(","))
                {
                    return Json(new { success = false, message = "Role field cannot be more than 1!" });
                }
                _context.ApplicationUser.Update(updateApplicationUser);

            }

            if (userRoleId == 0)
            {
                UserRole newUserRole = new UserRole
                {
                    RoleId = updateApplicationUser.RoleId,
                    UserId = updateApplicationUser.UserId,
                    Modules = updateApplicationUser.Modules
                };
                _context.UserRole.Add(newUserRole);
                }
                else
                {
                    UserRole updateUserRole = _context.UserRole.Where(x => x.UserId == id).FirstOrDefault();
                    {
                        updateUserRole.RoleId = applicationUser.RoleId;
                        updateUserRole.Modules = applicationUser.Modules;
      
                        _context.UserRole.Update(updateUserRole);
                    }
                }
            
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/GetUserRoleByUserID
        [HttpPost("GetUserRoleByUserID")]
        public IActionResult GetUserRoleByUserID(int userId)
        {
            UserRole userRole = _context.UserRole.Where(x => x.UserId == userId).FirstOrDefault();
            var userSelectedRoles = "";
            if (userRole != null && userRole.RoleId != null)
            {
                userSelectedRoles = userRole.RoleId;
            }
            List<Roles> listRole = _context.Role.ToList();

            if (userSelectedRoles != "")
            {
                var query = from val in userSelectedRoles.Split(',')
                            select (val);
                foreach (var item in listRole)
                {
                    bool containsItem = query.Any(x => x == item.Name);
                    if (containsItem)
                        item.Selected = true;
                }
            }
            return Json(new { data = listRole });
        }

        // POST: api/Roles/DeactivateUser
        [HttpPost("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var info = await _userManager.GetUserAsync(User);
            UserRole userRole = _context.UserRole.Where(x => x.UserId == id).FirstOrDefault();
            {
                userRole.RoleId = "Deactivated";
                userRole.Modules = null;
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.RoleId = "Deactivated";
                applicationUser.Modules = null;
                applicationUser.Modifier = info.FullName;
            }

            _context.UserRole.Update(userRole);
            _context.ApplicationUser.Update(applicationUser);

            EditedDatas editedDatas = new EditedDatas
            {
                DateEdited = DateTime.Now,
                Origin = "Users/Deactivate",
                EditedBy = info.FullName,
                ControlNumber = applicationUser.UserId
            };
            //editedDatas.Remarks = applicationUser.remar;
            _context.EditedDatas.Add(editedDatas);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account has been Deactivated!" });
        }

        // POST: api/Roles/DeleteAccess
        [HttpPost("DeleteAccess")]
        public async Task<IActionResult> DeleteAccess(int id)
        {
            var info = await _userManager.GetUserAsync(User);
            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.isAdmin = true;
                applicationUser.Modifier = info.FullName;
            }

            _context.ApplicationUser.Update(applicationUser);

            EditedDatas editedDatas = new EditedDatas
            {
                DateEdited = DateTime.Now,
                Origin = "Users/Extra priveledge",
                EditedBy = info.FullName,
                ControlNumber = applicationUser.UserId
            };
            editedDatas.EditedData = "Without extra priveledge - With extra priveledge";
            _context.EditedDatas.Add(editedDatas);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account has been given a delete access!" });
        }

        // POST: api/Roles/RemoveDeleteAccess
        [HttpPost("RemoveDeleteAccess")]
        public async Task<IActionResult> RemoveDeleteAccess(int id)
        {
            var info = await _userManager.GetUserAsync(User);
            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.isAdmin = false;
                applicationUser.Modifier = info.FullName;
            }
            _context.ApplicationUser.Update(applicationUser);

            EditedDatas editedDatas = new EditedDatas
            {
                DateEdited = DateTime.Now,
                Origin = "Users/Remove extra priveledge",
                EditedBy = info.FullName,
                ControlNumber = applicationUser.UserId
            };
            editedDatas.EditedData = "With extra priveledge - Without extra priveledge";
            //editedDatas.Remarks = applicationUser.remar;
            _context.EditedDatas.Add(editedDatas);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account delete access has been removed!" });
        }

    }
}