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
    [Route("api/Accreditation")]
    //[Authorize]
    public class AccreditationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public AccreditationController(ApplicationDbContext context,
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

        // GET: api/Accreditation/GetInterTraders
        [HttpGet("GetInterTraders")]
        public IActionResult GetInterTraders([FromRoute]Guid organizationId)
        {
            var interTraders = _context.AccreditedInterTraders.OrderByDescending(x => x.DateOfApplication).ToList();
            return Json(new { data = interTraders });
        }

        // GET: api/Accreditation/GetPackersAndPorters
        [HttpGet("GetPackersAndPorters")]
        public IActionResult GetPackersAndPorters([FromRoute]Guid organizationId)
        {
            var packersAndPorters = _context.AccreditedPackersAndPorters.OrderByDescending(x => x.DateOfApplication).ToList();
            return Json(new { data = packersAndPorters });
        }

        // GET: api/Accreditation/GetBuyers
        [HttpGet("GetBuyers")]
        public IActionResult GetBuyers([FromRoute]Guid organizationId)
        {
            var buyers = _context.AccreditedBuyers.OrderByDescending(x => x.DateOfApplication).ToList();
            return Json(new { data = buyers });
        }

        // GET: api/Accreditation/GetMarketFacilitators
        [HttpGet("GetMarketFacilitators")]
        public IActionResult GetMarketFacilitators([FromRoute]Guid organizationId)
        {
            var marketFacilitators = _context.AccreditedMarketFacilitators.OrderByDescending(x => x.DateOfApplication).ToList();
            return Json(new { data = marketFacilitators });
        }

        // GET: api/Accreditation/GetIndividualFarmers
        [HttpGet("GetIndividualFarmers")]
        public IActionResult GetIndividualFarmers([FromRoute]Guid organizationId)
        {
            var individualFarmers = _context.AccreditedIndividualFarmers.OrderByDescending(x => x.DateOfApplication).ToList();
            return Json(new { data = individualFarmers });
        }

        // POST: api/Accreditation/PostInterTraders
        [HttpPost("PostInterTraders")]
        public async Task<IActionResult> PostInterTraders([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            InterTraders interTraders = new InterTraders
            {
                DateOfApplication = DateTime.Now,
                Counter = model["Counter"].ToString(),
                NameOfAssociation = model["NameOfAssociation"].ToString(),
                ReferenceNumber = Convert.ToInt32(model["ReferenceNumber"].ToString()),
                IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Name = model["Name"].ToString(),
                NameOfSpouse = model["NameOfSpouse"].ToString(),
                PresentAddress = model["PresentAddress"].ToString(),
                ContactNumber = model["ContactNumber"].ToString(),
                BusinessPermit = model["BusinessPermit"].ToString(),
                Tin = Convert.ToInt32(model["Tin"].ToString()),
                Destination = model["Destination"].ToString()
            };
            if (id == 0)
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    interTraders.Barangay = addresses.Barangay;
                    interTraders.Municipality = addresses.Municipality;
                    interTraders.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedInterTraders.Add(interTraders);
            }
            else
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    interTraders.Barangay = addresses.Barangay;
                    interTraders.Municipality = addresses.Municipality;
                    interTraders.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                interTraders.Id = id;
                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedInterTraders.Update(interTraders);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Accreditation/PostPackersAndPorters
        [HttpPost("PostPackersAndPorters")]
        public async Task<IActionResult> PostPackersAndPorters([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            PackersAndPorters packersAndPorters = new PackersAndPorters
            {
                DateOfApplication = DateTime.Now,
                NameOfAssociation = model["NameOfAssociation"].ToString(),
                PackerOrPorter = model["PackerOrPorter"].ToString(),
                IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Name = model["Name"].ToString(),
                NickName = model["NickName"].ToString(),
                NameOfSpouse = model["NameOfSpouse"].ToString(),
                PresentAddress = model["PresentAddress"].ToString(),
                ContactNumber = model["ContactNumber"].ToString(),
                BirthDate = model["BirthDate"].ToString(),
                ProvincialAddress = model["ProvincialAddress"].ToString(),
                Requirements = model["Requirements"].ToString()
            };
            if (id == 0)
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    packersAndPorters.Barangay = addresses.Barangay;
                    packersAndPorters.Municipality = addresses.Municipality;
                    packersAndPorters.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedPackersAndPorters.Add(packersAndPorters);
            }
            else
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    packersAndPorters.Barangay = addresses.Barangay;
                    packersAndPorters.Municipality = addresses.Municipality;
                    packersAndPorters.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                packersAndPorters.Id = id;
                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedPackersAndPorters.Update(packersAndPorters);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Accreditation/PostBuyers
        [HttpPost("PostBuyers")]
        public async Task<IActionResult> PostBuyers([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            Buyers buyers = new Buyers
            {
                //DateOfApplication = DateTime.Now,
                NameOfSpouse = model["NameOfSpouse"].ToString(),
                PresentAddress = model["PresentAddress"].ToString(),
                ContactNumber = model["ContactNumber"].ToString(),
                BirthDate = model["BirthDate"].ToString(),
                Tin = Convert.ToInt32(model["Tin"].ToString()),
                BusinessName = model["BusinessName"].ToString(),
                BusinessAddress = model["BusinessAddress"].ToString(),
                VehiclePlateNumber = model["VehiclePlateNumber"].ToString(),
                ProductDestination = model["ProductDestination"].ToString(),
                DateOfApplication = DateTime.Now
            };
            if (id == 0)
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    buyers.Barangay = addresses.Barangay;
                    buyers.Municipality = addresses.Municipality;
                    buyers.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedBuyers.Add(buyers);
            }
            else
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    buyers.Barangay = addresses.Barangay;
                    buyers.Municipality = addresses.Municipality;
                    buyers.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                buyers.Id = id;
                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedBuyers.Update(buyers);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Accreditation/PostMarketFacilitators
        [HttpPost("PostMarketFacilitators")]
        public async Task<IActionResult> PostMarketFacilitators([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            MarketFacilitators marketFacilitators = new MarketFacilitators
            {
                DateOfApplication = DateTime.Now,
                NameOfAssociation = model["NameOfAssociation"].ToString(),
                ReferenceNumber = Convert.ToInt32(model["ReferenceNumber"].ToString()),
                IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Name = model["Name"].ToString(),
                NickName = model["NickName"].ToString(),
                NameOfSpouse = model["NameOfSpouse"].ToString(),
                PresentAddress = model["PresentAddress"].ToString(),
                ContactNumber = model["ContactNumber"].ToString(),
                BirthDate = model["BirthDate"].ToString(),
                Tin = Convert.ToInt32(model["Tin"].ToString()),
                BusinessName = model["BusinessName"].ToString(),
                BusinessAddress = model["BusinessAddress"].ToString(),
                PlateNumber = model["PlateNumber"].ToString()
            };

            Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
            if (addresses != null)
            {
                marketFacilitators.Barangay = addresses.Barangay;
                marketFacilitators.Municipality = addresses.Municipality;
                marketFacilitators.Province = addresses.Province;
            }
            else
            {
                return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
            }

            Commodities commodities = _context.Commodities.Where(x => x.Commodity == model["MajorCommodity"].ToString()).FirstOrDefault();
            if (commodities != null)
            {
                marketFacilitators.MajorCommodity = commodities.Commodity;
            }
            else
            {
                return Json(new { success = false, message = "Add commodity to Settings/Commodities!" });
            }

            if (id == 0)
            {
                _context.AccreditedMarketFacilitators.Add(marketFacilitators);
            }
            else
            {
                marketFacilitators.Id = id;
                _context.AccreditedMarketFacilitators.Update(marketFacilitators);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/Accreditation/PostIndividualFarmers
        [HttpPost("PostIndividualFarmers")]
        public async Task<IActionResult> PostIndividualFarmers([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            IndividualFarmers individualFarmers = new IndividualFarmers
            {
                DateOfApplication = DateTime.Now,
                Counter = model["Counter"].ToString(),
                Association = model["Association"].ToString(),
                ReferenceNumber = Convert.ToInt32(model["ReferenceNumber"].ToString()),
                IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                Name = model["Name"].ToString(),
                SpouseName = model["SpouseName"].ToString(),
                ContactNumber = model["ContactNumber"].ToString(),
                BirthDate = model["BirthDate"].ToString(),
                PlateNumber = model["PlateNumber"].ToString(),
                EstimatedTotalLandArea = model["EstimatedTotalLandArea"].ToString(),
                MajorCrops = model["MajorCrops"].ToString(),
                LandAreaPerCrop = model["LandAreaPerCrop"].ToString(),
                EstimatedProduce = Convert.ToInt32(model["EstimatedProduce"].ToString()),
                Planting = model["Planting"].ToString(),
                Harvesting = model["Harvesting"].ToString()
            };
            if (id == 0)
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    individualFarmers.Barangay = addresses.Barangay;
                    individualFarmers.Municipality = addresses.Municipality;
                    individualFarmers.Province = addresses.Province;
                    individualFarmers.Sitio = "";
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedIndividualFarmers.Add(individualFarmers);
            }
            else
            {
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    individualFarmers.Barangay = addresses.Barangay;
                    individualFarmers.Municipality = addresses.Municipality;
                    individualFarmers.Province = addresses.Province;
                    individualFarmers.Sitio = "";
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                individualFarmers.Id = id;
                //securityInspectionReport.Inspector = info.FullName;
                _context.AccreditedIndividualFarmers.Update(individualFarmers);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/Accreditation/DeleteInterTraders
        [HttpDelete("InterTraders/{id}")]
        public async Task<IActionResult> DeleteInterTraders([FromRoute] int id)
        {
            InterTraders interTraders = _context.AccreditedInterTraders.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(interTraders);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeletePackersAndPorters
        [HttpDelete("DeletePackersAndPorters/{id}")]
        public async Task<IActionResult> DeletePackersAndPorters([FromRoute] int id)
        {
            PackersAndPorters packersAndPorters = _context.AccreditedPackersAndPorters.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(packersAndPorters);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeleteBuyers
        [HttpDelete("DeleteBuyers/{id}")]
        public async Task<IActionResult> DeleteBuyers([FromRoute] int id)
        {
            Buyers buyers = _context.AccreditedBuyers.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(buyers);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeleteMarketFacilitators
        [HttpDelete("DeleteMarketFacilitators/{id}")]
        public async Task<IActionResult> DeleteMarketFacilitators([FromRoute] int id)
        {
            MarketFacilitators marketFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(marketFacilitators);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeleteIndividualFarmers
        [HttpDelete("DeleteIndividualFarmers/{id}")]
        public async Task<IActionResult> DeleteIndividualFarmers([FromRoute] int id)
        {
            IndividualFarmers individualFarmers = _context.AccreditedIndividualFarmers.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(individualFarmers);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}