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
    [Route("api/TradingInspector")]
    //[Authorize]
    public class TradingInspectorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public TradingInspectorController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: api/TradingInspector
        [HttpGet("{organizationId}")]
        public IActionResult GetTradingInspector([FromRoute]Guid organizationId)
        {
            var ticketing = _context.Ticketing.ToList();
            return Json(new { data = ticketing });
        }


        // POST: api/TradingInspector
        [HttpPost]
        public async Task<IActionResult> PostTradingInspector([FromBody] JObject model)
        {
            Guid objGuid = Guid.Empty;
            objGuid = Guid.Parse(model["ticketingId"].ToString());
            Ticketing ticketing = new Ticketing
            {
                timeIn = Convert.ToDateTime(model["timeIn"].ToString()),
                timeOut = Convert.ToDateTime(model["timeOut"].ToString()),
                plateNumber = model["plateNumber"].ToString(),
                typeOfTransaction = model["typeOfTransaction"].ToString(),
                gatePassDate = model["gatePassDate"].ToString()
            };
            if (objGuid == Guid.Empty)
            {
                ticketing.ticketingId = Guid.NewGuid();
                _context.Ticketing.Add(ticketing);
            }
            else
            {
                ticketing.ticketingId = objGuid;
                _context.Ticketing.Update(ticketing);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        // DELETE: api/TradingInspector/
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTradingInspector([FromRoute] Guid id)
        {
            Ticketing ticketing = _context.Ticketing.Where(x => x.ticketingId == id).FirstOrDefault();
            _context.Remove(ticketing);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }
    }
}