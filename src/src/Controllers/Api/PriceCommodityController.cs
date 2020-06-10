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

        // GET: api/PriceCommodity
        [HttpGet("{organizationId}")]
        public IActionResult GetPriceCommodity([FromRoute]Guid organizationId)
        {
            var commodities = _context.Commodities.ToList();
            return Json(new { data = commodities });
        }

        // GET: api/PriceCommodity
        [HttpGet("GetPrice")]
        public IActionResult GetPrice([FromRoute]Guid organizationId)
        {
            var price = _context.PriceCommodity.ToList();
            return Json(new { data = price });
        }

        // POST: api/SupportEngineer
        [HttpPost]
        public async Task<IActionResult> PostPriceCommodity([FromBody] JObject model)
        {
            PriceCommodity priceCommodity = new PriceCommodity
            {
                //time = Convert.ToDateTime(model["time"].ToString()),
                commodityDate = DateTime.Now,
                commodity = model["Commodity"].ToString(),
                commodityRemarks = model["Remarks"].ToString(),
                priceRange = Convert.ToDouble(model["Price"].ToString()),
                classVariety = model["ClassVariety"].ToString()
            };
            priceCommodity.priceCommodityId = Guid.NewGuid();
            _context.PriceCommodity.Add(priceCommodity);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // POST: api/PriceCommodity/EditPrice
        [HttpPost("EditPrice")]
        public async Task<IActionResult> EditPrice([FromBody] JObject model)
        {
            PriceCommodity currentPriceCommodity = _context.PriceCommodity.Where(x => x.priceCommodityId == Guid.Parse(model["priceCommodityId"].ToString())).FirstOrDefault();
            var today = currentPriceCommodity.commodityDate.Date;
            var check = DateTime.Now.Date;
            if (today != check)
            {
                return Json(new { success = false, message = "Can only edit datas saved today!" });
            }
            currentPriceCommodity.commodityDate = DateTime.Now;
            currentPriceCommodity.commodityRemarks = model["commodityRemarks"].ToString();
            currentPriceCommodity.priceRange = Convert.ToDouble(model["priceRange"].ToString());
            currentPriceCommodity.classVariety = model["classVariety"].ToString();
            _context.PriceCommodity.Update(currentPriceCommodity);
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

    }
}