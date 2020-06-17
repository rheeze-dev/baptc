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
            Guid objGuid = Guid.Empty;
            var averageLow = _context.PriceCommodity.Where(x => x.commodityDate.Day == DateTime.Now.Day && x.commodity == model["commodity"].ToString() && x.classVariety == model["classVariety"].ToString()).Select(x => x.priceLow).Sum();
            var averageHigh = _context.PriceCommodity.Where(x => x.commodityDate.Day == DateTime.Now.Day && x.commodity == model["commodity"].ToString() && x.classVariety == model["classVariety"].ToString()).Select(x => x.priceHigh).Sum();
            objGuid = Guid.Parse(model["priceCommodityId"].ToString());

            if (objGuid == Guid.Empty)
            {
                PriceCommodity priceCommodity = new PriceCommodity
                {
                    //time = Convert.ToDateTime(model["time"].ToString()),
                    commodityDate = DateTime.Now,
                    commodity = model["commodity"].ToString(),
                    commodityRemarks = model["commodityRemarks"].ToString(),
                    priceLow = Convert.ToDouble(model["priceLow"].ToString()),
                    priceHigh = Convert.ToDouble(model["priceHigh"].ToString()),
                    classVariety = model["classVariety"].ToString()
                };

                var currentTotalDays = _context.PriceCommodity.OrderByDescending(x => x.commodityDate).Where(x => x.commodity == model["commodity"].ToString() && x.classVariety == model["classVariety"].ToString()).Select(x => x.totalDays).FirstOrDefault();
                var countDays = _context.PriceCommodity.Where(x => x.commodity == model["commodity"].ToString() && x.classVariety == model["classVariety"].ToString()).Count();
                var monthChecker = _context.PriceCommodity.OrderByDescending(x => x.commodityDate).Where(x => x.commodity == model["commodity"].ToString() && x.classVariety == model["classVariety"].ToString()).Select(x => x.commodityDate).FirstOrDefault();
                var latestMonth = monthChecker.Month;
                if (currentTotalDays == 0)
                {
                    priceCommodity.totalDays = 1;
                    priceCommodity.averageLow = priceCommodity.priceLow / priceCommodity.totalDays;
                    priceCommodity.averageHigh = priceCommodity.priceHigh / priceCommodity.totalDays;
                }
                else if (DateTime.Now.Month == latestMonth)
                {
                    priceCommodity.totalDays = countDays + 1;
                    priceCommodity.averageLow = (priceCommodity.priceLow + averageLow) / priceCommodity.totalDays;
                    priceCommodity.averageHigh = (priceCommodity.priceHigh + averageHigh) / priceCommodity.totalDays;
                }
                else
                {
                    priceCommodity.totalDays = 1;
                    priceCommodity.averageLow = priceCommodity.priceLow / priceCommodity.totalDays;
                    priceCommodity.averageHigh = priceCommodity.priceHigh / priceCommodity.totalDays;
                }

                priceCommodity.priceCommodityId = Guid.NewGuid();
                _context.PriceCommodity.Add(priceCommodity);
            }
            else
            {
                var currentPrice = _context.PriceCommodity.Where(x => x.priceCommodityId == objGuid).FirstOrDefault();
                if (model["commodity"].ToString() != currentPrice.commodity)
                {
                    return Json(new { success = false, message = "Commodity cannot be changed!" });
                }
                if (model["classVariety"].ToString() != currentPrice.classVariety)
                {
                    return Json(new { success = false, message = "Class variety cannot be changed!" });
                }
                var check = _context.PriceCommodity.OrderByDescending(x => x.commodityDate).Where(x => x.commodity == model["commodity"].ToString() && x.classVariety == model["classVariety"].ToString()).Select(x => x.priceCommodityId).FirstOrDefault();
                if (check != currentPrice.priceCommodityId)
                {
                    return Json(new { success = false, message = "You are only allowed to change the latest datas!" });
                }
                var oldLowPrice = currentPrice.priceLow;
                var oldHighPrice = currentPrice.priceHigh;
                currentPrice.priceLow = Convert.ToDouble(model["priceLow"].ToString());
                currentPrice.priceHigh = Convert.ToDouble(model["priceHigh"].ToString());
                currentPrice.commodityRemarks = model["commodityRemarks"].ToString();
                currentPrice.averageLow = ((averageLow - oldLowPrice ) + currentPrice.priceLow) / currentPrice.totalDays;
                currentPrice.averageHigh = ((averageHigh - oldHighPrice) + currentPrice.priceHigh) / currentPrice.totalDays;

                _context.PriceCommodity.Update(currentPrice);
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

            var info = await _userManager.GetUserAsync(User);
            DeletedDatas deleted = new DeletedDatas
            {
                DateDeleted = DateTime.Now,
                PlateNumber = "",
                Origin = "Price commodity",
                Name = priceCommodity.commodity,
                DeletedBy = info.FullName,
                Remarks = priceCommodity.commodityRemarks
            };

            _context.DeletedDatas.Add(deleted);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}