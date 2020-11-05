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
            var interTraders = _context.AccreditedInterTraders.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = interTraders });
        }

        // GET: api/Accreditation/GetPackersAndPorters
        [HttpGet("GetPackersAndPorters")]
        public IActionResult GetPackersAndPorters([FromRoute]Guid organizationId)
        {
            var packersAndPorters = _context.AccreditedPackersAndPorters.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = packersAndPorters });
        }

        // GET: api/Accreditation/GetBuyers
        [HttpGet("GetBuyers")]
        public IActionResult GetBuyers([FromRoute]Guid organizationId)
        {
            var buyers = _context.AccreditedBuyers.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = buyers });
        }

        // GET: api/Accreditation/GetMarketFacilitators
        [HttpGet("GetMarketFacilitators")]
        public IActionResult GetMarketFacilitators([FromRoute]Guid organizationId)
        {
            var marketFacilitators = _context.AccreditedMarketFacilitators.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = marketFacilitators });
        }

        // GET: api/Accreditation/GetIndividualFarmers
        [HttpGet("GetIndividualFarmers")]
        public IActionResult GetIndividualFarmers([FromRoute]Guid organizationId)
        {
            var individualFarmers = _context.AccreditedIndividualFarmers.OrderByDescending(x => x.Id).ToList();
            return Json(new { data = individualFarmers });
        }

        // POST: api/Accreditation/PostInterTraders
        [HttpPost("PostInterTraders")]
        public async Task<IActionResult> PostInterTraders([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());
            
            if (id == 0)
            {
                InterTraders interTraders = new InterTraders
                {
                    DateOfApplication = DateTime.Now,
                    Counter = model["Counter"].ToString(),
                    NameOfAssociation = model["NameOfAssociation"].ToString(),
                    ReferenceNumber = model["ReferenceNumber"].ToString(),
                    IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                    Name = model["Name"].ToString(),
                    NameOfSpouse = model["NameOfSpouse"].ToString(),
                    PresentAddress = model["PresentAddress"].ToString(),
                    ContactNumber = model["ContactNumber"].ToString(),
                    BusinessPermit = model["BusinessPermit"].ToString(),
                    Tin = model["Tin"].ToString(),
                    Destination = model["Destination"].ToString(),
                    Remarks = model["Remarks"].ToString()
                };
                interTraders.EnteredBy = info.FullName;

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
                var currentInterTraders = _context.AccreditedInterTraders.Where(x => x.Id == id).FirstOrDefault();
                if (currentInterTraders.Counter != model["Counter"].ToString() || currentInterTraders.NameOfAssociation != model["NameOfAssociation"].ToString() || currentInterTraders.ReferenceNumber != model["ReferenceNumber"].ToString() || currentInterTraders.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()) || currentInterTraders.Name != model["Name"].ToString() || currentInterTraders.NameOfSpouse != model["NameOfSpouse"].ToString() || currentInterTraders.PresentAddress != model["PresentAddress"].ToString() || currentInterTraders.ContactNumber != model["ContactNumber"].ToString() || currentInterTraders.BusinessPermit != model["BusinessPermit"].ToString() || currentInterTraders.Tin != model["Tin"].ToString() || currentInterTraders.Destination != model["Destination"].ToString() || currentInterTraders.Remarks != model["Remarks"].ToString() || currentInterTraders.Barangay != model["Barangay"].ToString())
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
                    var eleven = "";
                    var twelve = "";
                    var thirteen = "";
                    if (currentInterTraders.Counter != model["Counter"].ToString())
                    {
                        one = "Counter = " + currentInterTraders.Counter + " - " + model["Counter"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentInterTraders.NameOfAssociation != model["NameOfAssociation"].ToString())
                    {
                        two = " Name of association = " + currentInterTraders.NameOfAssociation + " - " + model["NameOfAssociation"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentInterTraders.ReferenceNumber != model["ReferenceNumber"].ToString())
                    {
                        three = " Reference number = " + currentInterTraders.ReferenceNumber + " - " + model["ReferenceNumber"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentInterTraders.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()))
                    {
                        four = " Id number = " + currentInterTraders.IdNumber + " - " + model["IdNumber"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentInterTraders.NameOfSpouse != model["NameOfSpouse"].ToString())
                    {
                        five = " Name of spouse = " + currentInterTraders.NameOfSpouse + " - " + model["NameOfSpouse"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentInterTraders.PresentAddress != model["PresentAddress"].ToString())
                    {
                        six = " Present address = " + currentInterTraders.PresentAddress + " - " + model["PresentAddress"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    if (currentInterTraders.Barangay != model["Barangay"].ToString())
                    {
                        seven = " Barangay = " + currentInterTraders.Barangay + " - " + model["Barangay"].ToString() + ";";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (currentInterTraders.ReferenceNumber != model["ReferenceNumber"].ToString())
                    {
                        eight = " Reference number = " + currentInterTraders.ReferenceNumber + " - " + model["ReferenceNumber"].ToString() + ";";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (currentInterTraders.ContactNumber != model["ContactNumber"].ToString())
                    {
                        nine = " Contact number = " + currentInterTraders.ContactNumber + " - " + model["ContactNumber"].ToString() + ";";
                    }
                    else
                    {
                        nine = "";
                    }
                    if (currentInterTraders.BusinessPermit != model["BusinessPermit"].ToString())
                    {
                        ten = " Business permit = " + currentInterTraders.BusinessPermit + " - " + model["BusinessPermit"].ToString() + ";";
                    }
                    else
                    {
                        ten = "";
                    }
                    if (currentInterTraders.Tin != model["Tin"].ToString())
                    {
                        eleven = " Tin = " + currentInterTraders.Tin + " - " + model["Tin"].ToString() + ";";
                    }
                    else
                    {
                        eleven = "";
                    }
                    if (currentInterTraders.Destination != model["Destination"].ToString())
                    {
                        twelve = " Destination = " + currentInterTraders.Destination + " - " + model["Destination"].ToString() + ";";
                    }
                    else
                    {
                        twelve = "";
                    }
                    if (currentInterTraders.Remarks != model["Remarks"].ToString())
                    {
                        thirteen = " Remarks = " + currentInterTraders.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        thirteen = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine + ten + eleven + twelve + thirteen;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Inter traders",
                        EditedBy = info.FullName,
                        ControlNumber = currentInterTraders.Id
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    currentInterTraders.Barangay = addresses.Barangay;
                    currentInterTraders.Municipality = addresses.Municipality;
                    currentInterTraders.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }
                
                currentInterTraders.Counter = model["Counter"].ToString();
                currentInterTraders.NameOfAssociation = model["NameOfAssociation"].ToString();
                currentInterTraders.ReferenceNumber = model["ReferenceNumber"].ToString();
                currentInterTraders.IdNumber = Convert.ToInt32(model["IdNumber"].ToString());
                currentInterTraders.Name = model["Name"].ToString();
                currentInterTraders.NameOfSpouse = model["NameOfSpouse"].ToString();
                currentInterTraders.PresentAddress = model["PresentAddress"].ToString();
                currentInterTraders.ContactNumber = model["ContactNumber"].ToString();
                currentInterTraders.BusinessPermit = model["BusinessPermit"].ToString();
                currentInterTraders.Tin = model["Tin"].ToString();
                currentInterTraders.Destination = model["Destination"].ToString();
                currentInterTraders.Remarks = model["Remarks"].ToString();
                currentInterTraders.EnteredBy = info.FullName;
                _context.AccreditedInterTraders.Update(currentInterTraders);

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
            
            if (id == 0)
            {
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
                    Requirements = model["Requirements"].ToString(),
                    Remarks = model["Remarks"].ToString()
                };
                packersAndPorters.EnteredBy = info.FullName;
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
                var currentPackers = _context.AccreditedPackersAndPorters.Where(x => x.Id == id).FirstOrDefault();
                if (currentPackers.Name != model["Name"].ToString() || currentPackers.NameOfAssociation != model["NameOfAssociation"].ToString() || currentPackers.PackerOrPorter != model["PackerOrPorter"].ToString() || currentPackers.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()) || currentPackers.NickName != model["NickName"].ToString() || currentPackers.NameOfSpouse != model["NameOfSpouse"].ToString() || currentPackers.PresentAddress != model["PresentAddress"].ToString() || currentPackers.ContactNumber != model["ContactNumber"].ToString() || currentPackers.ProvincialAddress != model["ProvincialAddress"].ToString() || currentPackers.Barangay != model["Barangay"].ToString() || currentPackers.BirthDate != model["BirthDate"].ToString() || currentPackers.Remarks != model["Remarks"].ToString() || currentPackers.Requirements != model["Requirements"].ToString())
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
                    var eleven = "";
                    var twelve = "";
                    var thirteen = "";
                    if (currentPackers.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()))
                    {
                        one = "IdNumber = " + currentPackers.IdNumber + " - " + model["IdNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentPackers.Name != model["Name"].ToString())
                    {
                        two = " Name = " + currentPackers.Name + " - " + model["Name"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentPackers.NameOfAssociation != model["NameOfAssociation"].ToString())
                    {
                        three = " Name of association = " + currentPackers.NameOfAssociation + " - " + model["NameOfAssociation"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentPackers.PackerOrPorter != model["PackerOrPorter"].ToString())
                    {
                        four = " Type of work = " + currentPackers.PackerOrPorter + " - " + model["PackerOrPorter"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentPackers.NickName != model["NickName"].ToString())
                    {
                        five = " Nickname = " + currentPackers.NickName + " - " + model["NickName"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentPackers.NameOfSpouse != model["NameOfSpouse"].ToString())
                    {
                        six = " Name of spouse = " + currentPackers.NameOfSpouse + " - " + model["NameOfSpouse"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    if (currentPackers.ProvincialAddress != model["ProvincialAddress"].ToString())
                    {
                        seven = " Provincial address = " + currentPackers.ProvincialAddress + " - " + model["ProvincialAddress"].ToString() + ";";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (currentPackers.PresentAddress != model["PresentAddress"].ToString())
                    {
                        eight = " Present address = " + currentPackers.PresentAddress + " - " + model["PresentAddress"].ToString() + ";";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (currentPackers.Barangay != model["Barangay"].ToString())
                    {
                        nine = " Barangay = " + currentPackers.Barangay + " - " + model["Barangay"].ToString() + ";";
                    }
                    else
                    {
                        nine = "";
                    }
                    if (currentPackers.ContactNumber != model["ContactNumber"].ToString())
                    {
                        ten = " Contact number = " + currentPackers.ContactNumber + " - " + model["ContactNumber"].ToString() + ";";
                    }
                    else
                    {
                        ten = "";
                    }
                    if (currentPackers.BirthDate != model["BirthDate"].ToString())
                    {
                        eleven = " Birth date = " + currentPackers.BirthDate + " - " + model["BirthDate"].ToString() + ";";
                    }
                    else
                    {
                        eleven = "";
                    }
                    if (currentPackers.Requirements != model["Requirements"].ToString())
                    {
                        twelve = " Requirements = " + currentPackers.Requirements + " - " + model["Requirements"].ToString() + ";";
                    }
                    else
                    {
                        twelve = "";
                    }
                    if (currentPackers.Remarks != model["Remarks"].ToString())
                    {
                        thirteen = " Remarks = " + currentPackers.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        thirteen = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine + ten + eleven + twelve + thirteen;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Packers and porters",
                        EditedBy = info.FullName,
                        ControlNumber = currentPackers.Id
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    currentPackers.Barangay = addresses.Barangay;
                    currentPackers.Municipality = addresses.Municipality;
                    currentPackers.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }
                
                currentPackers.NameOfAssociation = model["NameOfAssociation"].ToString();
                currentPackers.PackerOrPorter = model["PackerOrPorter"].ToString();
                currentPackers.IdNumber = Convert.ToInt32(model["IdNumber"].ToString());
                currentPackers.Name = model["Name"].ToString();
                currentPackers.NickName = model["NickName"].ToString();
                currentPackers.NameOfSpouse = model["NameOfSpouse"].ToString();
                currentPackers.PresentAddress = model["PresentAddress"].ToString();
                currentPackers.ContactNumber = model["ContactNumber"].ToString();
                currentPackers.BirthDate = model["BirthDate"].ToString();
                currentPackers.ProvincialAddress = model["ProvincialAddress"].ToString();
                currentPackers.Requirements = model["Requirements"].ToString();
                currentPackers.Remarks = model["Remarks"].ToString();
                currentPackers.EnteredBy = info.FullName;
                _context.AccreditedPackersAndPorters.Update(currentPackers);
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
            
            if (id == 0)
            {
                Buyers buyers = new Buyers
                {
                    //DateOfApplication = DateTime.Now,
                    NameOfSpouse = model["NameOfSpouse"].ToString(),
                    PresentAddress = model["PresentAddress"].ToString(),
                    ContactNumber = model["ContactNumber"].ToString(),
                    BirthDate = model["BirthDate"].ToString(),
                    Tin = model["Tin"].ToString(),
                    BusinessName = model["BusinessName"].ToString(),
                    BusinessAddress = model["BusinessAddress"].ToString(),
                    VehiclePlateNumber = model["VehiclePlateNumber"].ToString(),
                    ProductDestination = model["ProductDestination"].ToString(),
                    DateOfApplication = DateTime.Now,
                    Remarks = model["Remarks"].ToString()
                };
                buyers.EnteredBy = info.FullName;

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
                var currentBuyers = _context.AccreditedBuyers.Where(x => x.Id == id).FirstOrDefault();
                if (currentBuyers.VehiclePlateNumber != model["VehiclePlateNumber"].ToString() || currentBuyers.NameOfSpouse != model["NameOfSpouse"].ToString() || currentBuyers.PresentAddress != model["PresentAddress"].ToString() || currentBuyers.Barangay != model["Barangay"].ToString() || currentBuyers.ContactNumber != model["ContactNumber"].ToString() || currentBuyers.BirthDate != model["BirthDate"].ToString() || currentBuyers.Tin != model["Tin"].ToString() || currentBuyers.BusinessName != model["BusinessName"].ToString() || currentBuyers.BusinessAddress != model["BusinessAddress"].ToString() || currentBuyers.ProductDestination != model["ProductDestination"].ToString() || currentBuyers.Remarks != model["Remarks"].ToString())
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
                    var eleven = "";
                    if (currentBuyers.VehiclePlateNumber != model["VehiclePlateNumber"].ToString())
                    {
                        one = "Vehicle plate number = " + currentBuyers.VehiclePlateNumber + " - " + model["VehiclePlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentBuyers.NameOfSpouse != model["NameOfSpouse"].ToString())
                    {
                        two = " Name of spouse = " + currentBuyers.NameOfSpouse + " - " + model["NameOfSpouse"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentBuyers.PresentAddress != model["PresentAddress"].ToString())
                    {
                        three = " Present address = " + currentBuyers.PresentAddress + " - " + model["PresentAddress"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentBuyers.Barangay != model["Barangay"].ToString())
                    {
                        four = " Barangay = " + currentBuyers.Barangay + " - " + model["Barangay"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentBuyers.ContactNumber != model["ContactNumber"].ToString())
                    {
                        five = " Contact number = " + currentBuyers.ContactNumber + " - " + model["ContactNumber"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentBuyers.BirthDate != model["BirthDate"].ToString())
                    {
                        six = " Birthdate = " + currentBuyers.BirthDate + " - " + model["BirthDate"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    if (currentBuyers.Tin != model["Tin"].ToString())
                    {
                        seven = " Tin = " + currentBuyers.Tin + " - " + model["Tin"].ToString() + ";";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (currentBuyers.BusinessName != model["BusinessName"].ToString())
                    {
                        eight = " Business name = " + currentBuyers.BusinessName + " - " + model["BusinessName"].ToString() + ";";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (currentBuyers.BusinessAddress != model["BusinessAddress"].ToString())
                    {
                        nine = " Business address = " + currentBuyers.BusinessAddress + " - " + model["BusinessAddress"].ToString() + ";";
                    }
                    else
                    {
                        nine = "";
                    }
                    if (currentBuyers.ProductDestination != model["ProductDestination"].ToString())
                    {
                        ten = " Product destination = " + currentBuyers.ProductDestination + " - " + model["ProductDestination"].ToString() + ";";
                    }
                    else
                    {
                        ten = "";
                    }
                    if (currentBuyers.Remarks != model["Remarks"].ToString())
                    {
                        eleven = " Remarks = " + currentBuyers.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        eleven = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine + ten + eleven;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Buyers",
                        EditedBy = info.FullName,
                        ControlNumber = currentBuyers.Id
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    currentBuyers.Barangay = addresses.Barangay;
                    currentBuyers.Municipality = addresses.Municipality;
                    currentBuyers.Province = addresses.Province;
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }
                currentBuyers.NameOfSpouse = model["NameOfSpouse"].ToString();
                currentBuyers.PresentAddress = model["PresentAddress"].ToString();
                currentBuyers.ContactNumber = model["ContactNumber"].ToString();
                currentBuyers.BirthDate = model["BirthDate"].ToString();
                currentBuyers.Tin = model["Tin"].ToString();
                currentBuyers.BusinessName = model["BusinessName"].ToString();
                currentBuyers.BusinessAddress = model["BusinessAddress"].ToString();
                currentBuyers.VehiclePlateNumber = model["VehiclePlateNumber"].ToString();
                currentBuyers.ProductDestination = model["ProductDestination"].ToString();
                currentBuyers.Remarks = model["Remarks"].ToString();
                currentBuyers.EnteredBy = info.FullName;
                _context.AccreditedBuyers.Update(currentBuyers);
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

            if (id == 0)
            {
                MarketFacilitators marketFacilitators = new MarketFacilitators
                {
                    DateOfApplication = DateTime.Now,
                    NameOfAssociation = model["NameOfAssociation"].ToString(),
                    ReferenceNumber = model["ReferenceNumber"].ToString(),
                    IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                    Name = model["Name"].ToString(),
                    NickName = model["NickName"].ToString(),
                    NameOfSpouse = model["NameOfSpouse"].ToString(),
                    PresentAddress = model["PresentAddress"].ToString(),
                    ContactNumber = model["ContactNumber"].ToString(),
                    BirthDate = model["BirthDate"].ToString(),
                    Tin = model["Tin"].ToString(),
                    BusinessName = model["BusinessName"].ToString(),
                    BusinessAddress = model["BusinessAddress"].ToString(),
                    PlateNumber = model["PlateNumber"].ToString(),
                    Remarks = model["Remarks"].ToString()
                };
                marketFacilitators.EnteredBy = info.FullName;

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
                _context.AccreditedMarketFacilitators.Add(marketFacilitators);
            }
            else
            {
                var currentFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.Id == id).FirstOrDefault();
                if (currentFacilitators.PlateNumber != model["PlateNumber"].ToString() || currentFacilitators.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()) || currentFacilitators.NameOfAssociation != model["NameOfAssociation"].ToString() || currentFacilitators.ReferenceNumber != model["ReferenceNumber"].ToString() || currentFacilitators.Name != model["Name"].ToString() || currentFacilitators.NickName != model["NickName"].ToString() || currentFacilitators.NameOfSpouse != model["NameOfSpouse"].ToString() || currentFacilitators.Tin != model["Tin"].ToString() || currentFacilitators.PresentAddress != model["PresentAddress"].ToString() || currentFacilitators.Barangay != model["Barangay"].ToString() || currentFacilitators.ContactNumber != model["ContactNumber"].ToString() || currentFacilitators.BirthDate != model["BirthDate"].ToString() || currentFacilitators.BusinessName != model["BusinessName"].ToString() || currentFacilitators.BusinessAddress != model["BusinessAddress"].ToString() || currentFacilitators.MajorCommodity != model["MajorCommodity"].ToString() || currentFacilitators.Remarks != model["Remarks"].ToString())
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
                    var eleven = "";
                    var twelve = "";
                    var thirteen = "";
                    var fourteen = "";
                    var fifteen = "";
                    var sixteen = "";

                    if (currentFacilitators.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "Plate number = " + currentFacilitators.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentFacilitators.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()))
                    {
                        two = " Id number = " + currentFacilitators.IdNumber + " - " + model["IdNumber"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentFacilitators.NameOfAssociation != model["NameOfAssociation"].ToString())
                    {
                        three = " Name of association = " + currentFacilitators.NameOfAssociation + " - " + model["NameOfAssociation"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentFacilitators.ReferenceNumber != model["ReferenceNumber"].ToString())
                    {
                        four = " Reference number = " + currentFacilitators.ReferenceNumber + " - " + model["ReferenceNumber"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentFacilitators.Name != model["Name"].ToString())
                    {
                        five = " Name = " + currentFacilitators.Name + " - " + model["Name"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentFacilitators.NickName != model["NickName"].ToString())
                    {
                        six = " Nickname = " + currentFacilitators.NickName + " - " + model["NickName"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    if (currentFacilitators.NameOfSpouse != model["NameOfSpouse"].ToString())
                    {
                        seven = " Name of spouse = " + currentFacilitators.NameOfSpouse + " - " + model["NameOfSpouse"].ToString() + ";";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (currentFacilitators.Tin != model["Tin"].ToString())
                    {
                        eight = " Tin = " + currentFacilitators.Tin + " - " + model["Tin"].ToString() + ";";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (currentFacilitators.PresentAddress != model["PresentAddress"].ToString())
                    {
                        nine = " Present address = " + currentFacilitators.PresentAddress + " - " + model["PresentAddress"].ToString() + ";";
                    }
                    else
                    {
                        nine = "";
                    }
                    if (currentFacilitators.Barangay != model["Barangay"].ToString())
                    {
                        ten = " Barangay = " + currentFacilitators.Barangay + " - " + model["Barangay"].ToString() + ";";
                    }
                    else
                    {
                        ten = "";
                    }
                    if (currentFacilitators.ContactNumber != model["ContactNumber"].ToString())
                    {
                        eleven = " Contact number = " + currentFacilitators.ContactNumber + " - " + model["ContactNumber"].ToString() + ";";
                    }
                    else
                    {
                        eleven = "";
                    }
                    if (currentFacilitators.BirthDate != model["BirthDate"].ToString())
                    {
                        twelve = " Birthdate = " + currentFacilitators.BirthDate + " - " + model["BirthDate"].ToString() + ";";
                    }
                    else
                    {
                        twelve = "";
                    }
                    if (currentFacilitators.BusinessName != model["BusinessName"].ToString())
                    {
                        thirteen = " Business name = " + currentFacilitators.BusinessName + " - " + model["BusinessName"].ToString() + ";";
                    }
                    else
                    {
                        thirteen = "";
                    }
                    if (currentFacilitators.BusinessAddress != model["BusinessAddress"].ToString())
                    {
                        fourteen = " Business address = " + currentFacilitators.BusinessAddress + " - " + model["BusinessAddress"].ToString() + ";";
                    }
                    else
                    {
                        fourteen = "";
                    }
                    if (currentFacilitators.MajorCommodity != model["MajorCommodity"].ToString())
                    {
                        fifteen = " Major commodity = " + currentFacilitators.MajorCommodity + " - " + model["MajorCommodity"].ToString() + ";";
                    }
                    else
                    {
                        fifteen = "";
                    }
                    if (currentFacilitators.Remarks != model["Remarks"].ToString())
                    {
                        sixteen = " Remarks = " + currentFacilitators.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        sixteen = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine + ten + eleven + twelve + thirteen + fourteen + fifteen + sixteen;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Market facilitators",
                        EditedBy = info.FullName,
                        ControlNumber = currentFacilitators.Id
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                currentFacilitators.NameOfAssociation = model["NameOfAssociation"].ToString();
                currentFacilitators.ReferenceNumber = model["ReferenceNumber"].ToString();
                currentFacilitators.IdNumber = Convert.ToInt32(model["IdNumber"].ToString());
                currentFacilitators.Name = model["Name"].ToString();
                currentFacilitators.NickName = model["NickName"].ToString();
                currentFacilitators.NameOfSpouse = model["NameOfSpouse"].ToString();
                currentFacilitators.PresentAddress = model["PresentAddress"].ToString();
                currentFacilitators.ContactNumber = model["ContactNumber"].ToString();
                currentFacilitators.BirthDate = model["BirthDate"].ToString();
                currentFacilitators.Tin = model["Tin"].ToString();
                currentFacilitators.BusinessName = model["BusinessName"].ToString();
                currentFacilitators.BusinessAddress = model["BusinessAddress"].ToString();
                currentFacilitators.PlateNumber = model["PlateNumber"].ToString();
                currentFacilitators.Remarks = model["Remarks"].ToString();
                currentFacilitators.EnteredBy = info.FullName;
                _context.AccreditedMarketFacilitators.Update(currentFacilitators);
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
            
            if (id == 0)
            {
                IndividualFarmers individualFarmers = new IndividualFarmers
                {
                    DateOfApplication = DateTime.Now,
                    Counter = model["Counter"].ToString(),
                    Association = model["Association"].ToString(),
                    ReferenceNumber = model["ReferenceNumber"].ToString(),
                    IdNumber = Convert.ToInt32(model["IdNumber"].ToString()),
                    Name = model["Name"].ToString(),
                    SpouseName = model["SpouseName"].ToString(),
                    ContactNumber = model["ContactNumber"].ToString(),
                    BirthDate = model["BirthDate"].ToString(),
                    PlateNumber = model["PlateNumber"].ToString(),
                    EstimatedTotalLandArea = model["EstimatedTotalLandArea"].ToString(),
                    MajorCrops = model["MajorCrops"].ToString(),
                    LandAreaPerCrop = model["LandAreaPerCrop"].ToString(),
                    EstimatedProduce = model["EstimatedProduce"].ToString(),
                    Planting = model["Planting"].ToString(),
                    Harvesting = model["Harvesting"].ToString(),
                    Remarks = model["Remarks"].ToString()
                };
                individualFarmers.EnteredBy = info.FullName;

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
                var currentFarmers = _context.AccreditedIndividualFarmers.Where(x => x.Id == id).FirstOrDefault();
                if (currentFarmers.PlateNumber != model["PlateNumber"].ToString() || currentFarmers.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()) || currentFarmers.Name != model["Name"].ToString() || currentFarmers.Association != model["Association"].ToString() || currentFarmers.ReferenceNumber != model["ReferenceNumber"].ToString() || currentFarmers.Counter != model["Counter"].ToString() || currentFarmers.SpouseName != model["SpouseName"].ToString() || currentFarmers.Barangay != model["Barangay"].ToString() || currentFarmers.ContactNumber != model["ContactNumber"].ToString() || currentFarmers.BirthDate != model["BirthDate"].ToString() || currentFarmers.EstimatedTotalLandArea != model["EstimatedTotalLandArea"].ToString() || currentFarmers.MajorCrops != model["MajorCrops"].ToString() || currentFarmers.LandAreaPerCrop != model["LandAreaPerCrop"].ToString() || currentFarmers.Planting != model["Planting"].ToString() || currentFarmers.Harvesting != model["Harvesting"].ToString() || currentFarmers.EstimatedProduce != model["EstimatedProduce"].ToString() || currentFarmers.Remarks != model["Remarks"].ToString())
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
                    var eleven = "";
                    var twelve = "";
                    var thirteen = "";
                    var fourteen = "";
                    var fifteen = "";
                    var sixteen = "";
                    var seventeen = "";

                    if (currentFarmers.PlateNumber != model["PlateNumber"].ToString())
                    {
                        one = "Plate number = " + currentFarmers.PlateNumber + " - " + model["PlateNumber"].ToString() + ";";
                    }
                    else
                    {
                        one = "";
                    }
                    if (currentFarmers.IdNumber != Convert.ToInt32(model["IdNumber"].ToString()))
                    {
                        two = " Id number = " + currentFarmers.IdNumber + " - " + model["IdNumber"].ToString() + ";";
                    }
                    else
                    {
                        two = "";
                    }
                    if (currentFarmers.Name != model["Name"].ToString())
                    {
                        three = " Name = " + currentFarmers.Name + " - " + model["Name"].ToString() + ";";
                    }
                    else
                    {
                        three = "";
                    }
                    if (currentFarmers.Association != model["Association"].ToString())
                    {
                        four = " Association = " + currentFarmers.Association + " - " + model["Association"].ToString() + ";";
                    }
                    else
                    {
                        four = "";
                    }
                    if (currentFarmers.ReferenceNumber != model["ReferenceNumber"].ToString())
                    {
                        five = " Reference number = " + currentFarmers.ReferenceNumber + " - " + model["ReferenceNumber"].ToString() + ";";
                    }
                    else
                    {
                        five = "";
                    }
                    if (currentFarmers.Counter != model["Counter"].ToString())
                    {
                        six = " Counter = " + currentFarmers.Counter + " - " + model["Counter"].ToString() + ";";
                    }
                    else
                    {
                        six = "";
                    }
                    if (currentFarmers.SpouseName != model["SpouseName"].ToString())
                    {
                        seven = " Spouse name = " + currentFarmers.SpouseName + " - " + model["SpouseName"].ToString() + ";";
                    }
                    else
                    {
                        seven = "";
                    }
                    if (currentFarmers.Barangay != model["Barangay"].ToString())
                    {
                        eight = " Barangay = " + currentFarmers.Barangay + " - " + model["Barangay"].ToString() + ";";
                    }
                    else
                    {
                        eight = "";
                    }
                    if (currentFarmers.ContactNumber != model["ContactNumber"].ToString())
                    {
                        nine = " ContactNumber = " + currentFarmers.ContactNumber + " - " + model["ContactNumber"].ToString() + ";";
                    }
                    else
                    {
                        nine = "";
                    }
                    if (currentFarmers.BirthDate != model["BirthDate"].ToString())
                    {
                        ten = " Birthdate = " + currentFarmers.BirthDate + " - " + model["BirthDate"].ToString() + ";";
                    }
                    else
                    {
                        ten = "";
                    }
                    if (currentFarmers.EstimatedTotalLandArea != model["EstimatedTotalLandArea"].ToString())
                    {
                        eleven = " Estimated land = " + currentFarmers.EstimatedTotalLandArea + " - " + model["EstimatedTotalLandArea"].ToString() + ";";
                    }
                    else
                    {
                        eleven = "";
                    }
                    if (currentFarmers.MajorCrops != model["MajorCrops"].ToString())
                    {
                        twelve = " Major crops = " + currentFarmers.MajorCrops + " - " + model["MajorCrops"].ToString() + ";";
                    }
                    else
                    {
                        twelve = "";
                    }
                    if (currentFarmers.LandAreaPerCrop != model["LandAreaPerCrop"].ToString())
                    {
                        thirteen = " Land area per crop = " + currentFarmers.LandAreaPerCrop + " - " + model["LandAreaPerCrop"].ToString() + ";";
                    }
                    else
                    {
                        thirteen = "";
                    }
                    if (currentFarmers.Planting != model["Planting"].ToString())
                    {
                        fourteen = " Planting = " + currentFarmers.Planting + " - " + model["Planting"].ToString() + ";";
                    }
                    else
                    {
                        fourteen = "";
                    }
                    if (currentFarmers.Harvesting != model["Harvesting"].ToString())
                    {
                        fifteen = " Harvesting = " + currentFarmers.Harvesting + " - " + model["Harvesting"].ToString() + ";";
                    }
                    else
                    {
                        fifteen = "";
                    }
                    if (currentFarmers.EstimatedProduce != model["EstimatedProduce"].ToString())
                    {
                        sixteen = " Estimated produce = " + currentFarmers.EstimatedProduce + " - " + model["EstimatedProduce"].ToString() + ";";
                    }
                    else
                    {
                        sixteen = "";
                    }
                    if (currentFarmers.Remarks != model["Remarks"].ToString())
                    {
                        seventeen = " Remarks = " + currentFarmers.Remarks + " - " + model["Remarks"].ToString() + ";";
                    }
                    else
                    {
                        seventeen = "";
                    }
                    var datas = one + two + three + four + five + six + seven + eight + nine + ten + eleven + twelve + thirteen + fourteen + fifteen + sixteen + seventeen;
                    EditedDatas editedDatas = new EditedDatas
                    {
                        DateEdited = DateTime.Now,
                        Origin = "Individual farmers",
                        EditedBy = info.FullName,
                        ControlNumber = currentFarmers.Id
                    };
                    editedDatas.EditedData = datas;
                    editedDatas.Remarks = model["Remarks"].ToString();
                    _context.EditedDatas.Add(editedDatas);
                }
                Addresses addresses = _context.Addresses.Where(x => x.Barangay == model["Barangay"].ToString()).FirstOrDefault();
                if (addresses != null)
                {
                    currentFarmers.Barangay = addresses.Barangay;
                    currentFarmers.Municipality = addresses.Municipality;
                    currentFarmers.Province = addresses.Province;
                    currentFarmers.Sitio = "";
                }
                else
                {
                    return Json(new { success = false, message = "Add barangay to Settings/Addresses!" });
                }

                currentFarmers.Counter = model["Counter"].ToString();
                currentFarmers.Association = model["Association"].ToString();
                currentFarmers.ReferenceNumber = model["ReferenceNumber"].ToString();
                currentFarmers.IdNumber = Convert.ToInt32(model["IdNumber"].ToString());
                currentFarmers.Name = model["Name"].ToString();
                currentFarmers.SpouseName = model["SpouseName"].ToString();
                currentFarmers.ContactNumber = model["ContactNumber"].ToString();
                currentFarmers.BirthDate = model["BirthDate"].ToString();
                currentFarmers.PlateNumber = model["PlateNumber"].ToString();
                currentFarmers.EstimatedTotalLandArea = model["EstimatedTotalLandArea"].ToString();
                currentFarmers.MajorCrops = model["MajorCrops"].ToString();
                currentFarmers.LandAreaPerCrop = model["LandAreaPerCrop"].ToString();
                currentFarmers.EstimatedProduce = model["EstimatedProduce"].ToString();
                currentFarmers.Planting = model["Planting"].ToString();
                currentFarmers.Harvesting = model["Harvesting"].ToString();
                currentFarmers.Remarks = model["Remarks"].ToString();
                currentFarmers.EnteredBy = info.FullName;
                _context.AccreditedIndividualFarmers.Update(currentFarmers);
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

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Inter-traders",
                Name = interTraders.Name,
                DeletedBy = info.FullName,
                Remarks = interTraders.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeletePackersAndPorters
        [HttpDelete("DeletePackersAndPorters/{id}")]
        public async Task<IActionResult> DeletePackersAndPorters([FromRoute] int id)
        {
            PackersAndPorters packersAndPorters = _context.AccreditedPackersAndPorters.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(packersAndPorters);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Packers and porters",
                Name = packersAndPorters.Name,
                DeletedBy = info.FullName,
                Remarks = packersAndPorters.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeleteBuyers
        [HttpDelete("DeleteBuyers/{id}")]
        public async Task<IActionResult> DeleteBuyers([FromRoute] int id)
        {
            Buyers buyers = _context.AccreditedBuyers.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(buyers);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = buyers.VehiclePlateNumber,
                Origin = "Buyers",
                Name = "",
                DeletedBy = info.FullName,
                Remarks = buyers.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeleteMarketFacilitators
        [HttpDelete("DeleteMarketFacilitators/{id}")]
        public async Task<IActionResult> DeleteMarketFacilitators([FromRoute] int id)
        {
            MarketFacilitators marketFacilitators = _context.AccreditedMarketFacilitators.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(marketFacilitators);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = marketFacilitators.PlateNumber,
                Origin = "Market facilitators",
                Name = marketFacilitators.Name,
                DeletedBy = info.FullName,
                Remarks = marketFacilitators.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

        // DELETE: api/Accreditation/DeleteIndividualFarmers
        [HttpDelete("DeleteIndividualFarmers/{id}")]
        public async Task<IActionResult> DeleteIndividualFarmers([FromRoute] int id)
        {
            IndividualFarmers individualFarmers = _context.AccreditedIndividualFarmers.Where(x => x.Id == id).FirstOrDefault();
            _context.Remove(individualFarmers);

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = individualFarmers.PlateNumber,
                Origin = "Individual farmers",
                Name = individualFarmers.Name,
                DeletedBy = info.FullName,
                Remarks = individualFarmers.Remarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}