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
            var roles = _context.Role.OrderByDescending(x => x.DateAdded).ToList();
            return Json(new { data = roles });
        }

        // GET: api/Roles
        [HttpGet("GetParkingNumbers")]
        public IActionResult GetParkingNumbers([FromRoute]Guid organizationId)
        {
            var parkingNumbers = _context.ParkingNumbers.OrderByDescending(x => x.DateAdded).ToList();
            return Json(new { data = parkingNumbers });
        }

        // GET: api/Roles/GetAddresses
        [HttpGet("GetAddresses")]
        public IActionResult GetAddresses([FromRoute]Guid organizationId)
        {
            var addresses = _context.Addresses.ToList();
            return Json(new { data = addresses });
        }

        // GET: api/Roles/GetCommodities
        [HttpGet("GetCommodities")]
        public IActionResult GetCommodities([FromRoute]Guid organizationId)
        {
            var commodties = _context.Commodities.ToList();
            return Json(new { data = commodties });
        }

        // GET: api/Roles
        [HttpGet("GetDeletedDatas")]
        public IActionResult GetDeletedDatas([FromRoute]Guid organizationId)
        {
            var deleted = _context.DeletedDatas.OrderByDescending(x => x.DateDeleted).ToList();
            return Json(new { data = deleted });
        }

        // POST: api/Roles/PostTicketingPrice
        [HttpPost("PostTicketingPrice")]
        public async Task<IActionResult> PostTicketingPrice([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            var info = await _userManager.GetUserAsync(User);

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
            };
            if (id == 0)
            {
                _context.TicketingPrice.Add(ticketingPrice);
            }
            else
            {
                ticketingPrice.Id = id;
                _context.TicketingPrice.Update(ticketingPrice);
            }
            ticketingPrice.Editor = info.FullName;
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostRoles
        [HttpPost]
        public async Task<IActionResult> PostRoles([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            Roles roles = new Roles
            {
                DateAdded = DateTime.Now,
                FullName = model["FullName"].ToString(),
                Remarks = model["Remarks"].ToString(),
                ShortName = model["ShortName"].ToString(),
                Name = model["ShortName"].ToString(),
            };
            if (id == 0)
            {
                _context.Role.Add(roles);
            }
            else
            {
                roles.Id = id;
                _context.Role.Update(roles);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostParkingNumbers
        [HttpPost("PostParkingNumbers")]
        public async Task<IActionResult> PostParkingNumbers([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            ParkingNumbers parkingNumbers = new ParkingNumbers
            {
                DateAdded = DateTime.Now,
                FullName = model["FullName"].ToString(),
                Remarks = model["Remarks"].ToString(),
                Name = model["Name"].ToString()
            };
            if (id == 0)
            {
                _context.ParkingNumbers.Add(parkingNumbers);
            }
            else
            {
                parkingNumbers.Id = id;
                _context.ParkingNumbers.Update(parkingNumbers);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostAddresses
        [HttpPost("PostAddresses")]
        public async Task<IActionResult> PostAddresses([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            Addresses addresses = new Addresses
            {
                Barangay = model["Barangay"].ToString(),
                Municipality = model["Municipality"].ToString(),
                Province = model["Province"].ToString(),
                Remarks = model["Remarks"].ToString()
            };
            if (id == 0)
            {
                _context.Addresses.Add(addresses);
            }
            else
            {
                addresses.Id = id;
                _context.Addresses.Update(addresses);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Roles/PostCommodities
        [HttpPost("PostCommodities")]
        public async Task<IActionResult> PostCommodities([FromBody] JObject model)
        {
            int id = 0;
            id = Convert.ToInt32(model["Id"].ToString());
            Commodities commodities = new Commodities
            {
                Commodity = model["Commodity"].ToString(),
                Remarks = model["Remarks"].ToString()
            };
            if (id == 0)
            {
                _context.Commodities.Add(commodities);
            }
            else
            {
                commodities.Id = id;
                _context.Commodities.Update(commodities);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Roles
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoles([FromRoute] int id)
        {
            Roles roles = _context.Role.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(roles);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles/DeleteParkingNumbers
        [HttpDelete("DeleteParkingNumbers/{id}")]
        public async Task<IActionResult> DeleteParkingNumbers([FromRoute] int id)
        {
            ParkingNumbers parking = _context.ParkingNumbers.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(parking);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Roles
        [HttpDelete("UserRoles/{id}")]
        public async Task<IActionResult> DeleteUserRoles([FromRoute] int id)
        {
            UserRole userRole = _context.UserRole.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(userRole);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // GET: api/Roles/GetUsers
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            List<ApplicationUser> listUser = _context.ApplicationUser.ToList();
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
            UserRole userRole = _context.UserRole.Where(x => x.UserId == id).FirstOrDefault();
            {
                userRole.RoleId = "Deactivated";
                userRole.Modules = null;
            }

            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.RoleId = "Deactivated";
                applicationUser.Modules = null;
            }

            _context.UserRole.Update(userRole);
            _context.ApplicationUser.Update(applicationUser);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account has been Deactivated!" });
        }

        // POST: api/Roles/DeleteAccess
        [HttpPost("DeleteAccess")]
        public async Task<IActionResult> DeleteAccess(int id)
        {
            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.isAdmin = true;
            }

            _context.ApplicationUser.Update(applicationUser);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account has been given a delete access!" });
        }

        // POST: api/Roles/RemoveDeleteAccess
        [HttpPost("RemoveDeleteAccess")]
        public async Task<IActionResult> RemoveDeleteAccess(int id)
        {
            ApplicationUser applicationUser = _context.ApplicationUser.Where(x => x.UserId == id).FirstOrDefault();
            {
                applicationUser.isAdmin = false;
            }

            _context.ApplicationUser.Update(applicationUser);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Account delete access has been removed!" });
        }

    }
}