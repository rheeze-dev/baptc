using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Services;

namespace src.Controllers
{
    public class PriceCommodityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISecurityService _securityService;

        public PriceCommodityController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ISecurityService securityService)
        {
            _context = context;
            _userManager = userManager;
            _securityService = securityService;
        }

        //public async Task<IActionResult> Index(Guid org)
        //{
        //    if (org == Guid.Empty)
        //    {
        //        return NotFound();
        //    }
        //    ApplicationUser appUser = await _userManager.GetUserAsync(User);
        //    var listModule = _securityService.ListModule(appUser);
        //    if (!listModule.Contains("PriceCommodities"))
        //    {
        //        return NotFound();
        //    }
        //    Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
        //    ViewData["org"] = org;
        //    return View(organization);
        //}

        public async Task<IActionResult> Price(Guid org)
        {
            if (org == Guid.Empty)
            {
                return NotFound();
            }
            ApplicationUser appUser = await _userManager.GetUserAsync(User);
            var listModule = _securityService.ListModule(appUser);
            if (!listModule.Contains("PriceCommodities"))
            {
                return NotFound();
            }
            Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            ViewData["org"] = org;
            return View(organization);
        }

        public async Task<IActionResult> Graph(Guid org)
        {
            var date = await _context.PriceCommodity.Select(c => c.commodityDate).Distinct().ToListAsync();
            var commodity = await _context.PriceCommodity.Select(c => c.commodity).Distinct().ToListAsync();
            var price = await _context.PriceCommodity.Select(c => c.priceRange).ToListAsync();
            //var averagePrice = price.Average();
            var firstClass = _context.PriceCommodity
                .Where(c => c.classVariety == "First class")
                .GroupBy(c => c.commodityDate)
                .Select(group => new
                {
                    CommodityDate = group.Key,
                    Count = group.Count()
                });
            var countFirstClass = firstClass.Select(a => a.Count).ToArray();

            var lowClass = _context.PriceCommodity
                .Where(c => c.classVariety == "Low class")
                .GroupBy(c => c.commodityDate)
                .Select(group => new
                {
                    CommodityDate = group.Key,
                    Count = group.Count()
                });
            var countLowClass = lowClass.Select(a => a.Count).ToArray();

            //var averagePriceFirstClass = _context.PriceCommodity.Where(c => c.commodity == )


            var priceFirstClass = _context.PriceCommodity
                .Where(c => c.commodity == "petsay" && c.classVariety == "First Class")
                .Average(c=> c.priceRange)
                //.Select(group => new
                //{
                //    CommodityDate = group.Key,
                //    Count = group.Count()
                //})
                ;
            var averagePriceFirstClass = "petsay:" + priceFirstClass;
            return new JsonResult(new { myDate = date, myFirstClass = countFirstClass, myLowClass = countLowClass, myCommodity = commodity, mYprice = averagePriceFirstClass });

            //if (org == Guid.Empty)
            //{
            //    return NotFound();
            //}
            //ApplicationUser appUser = await _userManager.GetUserAsync(User);
            //var listModule = _securityService.ListModule(appUser);
            //if (!listModule.Contains("PriceCommodities"))
            //{
            //    return NotFound();
            //}
            //Organization organization = _context.Organization.Where(x => x.organizationId.Equals(org)).FirstOrDefault();
            //ViewData["org"] = org;
            //return View(organization);
        }

        public async Task<IActionResult> BarGraph()
        {
            var date = await _context.PriceCommodity.Select(c => c.commodityDate).Distinct().ToListAsync();
            var FirstClass = _context.PriceCommodity
                .Where(c => c.classVariety == "First class")
                .GroupBy(c => c.commodityDate)
                .Select(group => new {
                    CommodityDate = group.Key,
                    Count = group.Count()
                });
            var countFirstClass = FirstClass.Select(a => a.Count).ToArray();
            var LowClass = _context.PriceCommodity
                .Where(c => c.classVariety == "Low class")
                .GroupBy(c => c.commodityDate)
                .Select(group => new {
                    CommodityDate = group.Key,
                    Count = group.Count()
                });
            var countLowClass = LowClass.Select(a => a.Count).ToArray();
            return new JsonResult(new { myDate = date, mySuccess = countFirstClass, myException = countLowClass });
        }

        public IActionResult AddEditPrice(Guid org, Guid id)
        {
            if (id == Guid.Empty)
            {
                PriceCommodity priceCommodity = new PriceCommodity();
                //priceCommodity.organizationId = org;
                return View(priceCommodity);
            }
            else
            {
                return View(_context.PriceCommodity.Where(x => x.priceCommodityId.Equals(id)).FirstOrDefault());
            }

        }
    }
}