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
    [Route("api/PriceCommodity")]
    //[Authorize]
    public class PriceCommodityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public PriceCommodityController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/SupportEngineer
        [HttpGet("{organizationId}")]
        public IActionResult GetPriceCommodity([FromRoute]Guid organizationId)
        {
            var priceCommodity = _context.PriceCommodity.ToList();
            return Json(new { data = priceCommodity });
        }


        // POST: api/SupportEngineer
        [HttpPost]
        public async Task<IActionResult> PostPriceCommodity([FromBody] JObject model)
        {
            Guid objGuid = Guid.Empty;
            objGuid = Guid.Parse(model["priceCommodityId"].ToString());
            PriceCommodity priceCommodity = new PriceCommodity
            {
                time = Convert.ToDateTime(model["time"].ToString()),
                commodityDate = Convert.ToDateTime(model["commodityDate"].ToString()),
                commodity = model["commodity"].ToString(),
                commodityRemarks = model["commodityRemarks"].ToString(),
                priceRange = Convert.ToDouble(model["priceRange"].ToString()),
                classVariety = model["classVariety"].ToString()
            };
            if (objGuid == Guid.Empty)
            {
                priceCommodity.priceCommodityId = Guid.NewGuid();
                _context.PriceCommodity.Add(priceCommodity);
            }
            else
            {
                priceCommodity.priceCommodityId = objGuid;
                _context.PriceCommodity.Update(priceCommodity);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/PriceCommodity/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriceCommodity([FromRoute] Guid id)
        {
            PriceCommodity priceCommodity = _context.PriceCommodity.Where(x => x.priceCommodityId == id).FirstOrDefault();
            _context.Remove(priceCommodity);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteSupportEngineer([FromRoute] Guid id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var supportEngineer = await _context.SupportEngineer.SingleOrDefaultAsync(m => m.supportEngineerId == id);
        //        if (supportEngineer == null)
        //        {
        //            return NotFound();
        //        }

        //        string applicationUserId = supportEngineer.applicationUserId;

        //        _context.SupportEngineer.Remove(supportEngineer);
        //        await _context.SaveChangesAsync();

        //        ApplicationUser appUser = await _userManager.FindByIdAsync(applicationUserId);
        //        await _userManager.DeleteAsync(appUser);

        //        return Json(new { success = true, message = "Delete success." });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { success = false, message = ex.Message });
        //    }


        //}

        //private bool SupportEngineerExists(Guid id)
        //{
        //    return _context.SupportEngineer.Any(e => e.supportEngineerId == id);
        //}
    }
}