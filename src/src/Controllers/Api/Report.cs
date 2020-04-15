using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using OfficeOpenXml;
using src.Data;
using src.Models;
using src.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace src.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Report")]
    //[Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public ReportController(ApplicationDbContext context,
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

        //[HttpPost("import")]
        //public async Task<DemoResponse<List<UserInfo>>> Import(IFormFile formFile, CancellationToken cancellationToken)
        //{
        //    if (formFile == null || formFile.Length <= 0)
        //    {
        //        return DemoResponse<List<UserInfo>>.GetResult(-1, "formfile is empty");
        //    }

        //    if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return DemoResponse<List<UserInfo>>.GetResult(-1, "Not Support file extension");
        //    }

        //    var list = new List<UserInfo>();

        //    using (var stream = new MemoryStream())
        //    {
        //        await formFile.CopyToAsync(stream, cancellationToken);

        //        using (var package = new ExcelPackage(stream))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            var rowCount = worksheet.Dimension.Rows;

        //            for (int row = 2; row <= rowCount; row++)
        //            {
        //                list.Add(new UserInfo
        //                {
        //                    UserName = worksheet.Cells[row, 1].Value.ToString().Trim(),
        //                    Age = int.Parse(worksheet.Cells[row, 2].Value.ToString().Trim()),
        //                });
        //            }
        //        }
        //    }

        //    // add list to db ..  
        //    // here just read and return  

        //    return DemoResponse<List<UserInfo>>.GetResult(0, "OK", list);
        //}

        [HttpGet("role")]
        public async Task<IActionResult> role(CancellationToken cancellationToken)
        {
            // query data from database  
            await Task.Yield();
            //List<PriceCommodity> priceCommodity = _context.PriceCommodity.ToList();
            var role = _context.Role.Select(x => new { Role = x.DateAdded.ToString("MMMM/dd/yyyy"), x.FullName, x.Module, x.Name, x.ShortName, x.Remarks }).ToList();


            //        var list = new List<UserInfo>()
            //{
            //    new UserInfo { UserName = "catcher", Age = 18 },
            //    new UserInfo { UserName = "james", Age = 20 },
            //};
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(role, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Role: {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> roles(int Id)
        {
            // query data from database  
            await Task.Yield();
            //List<PriceCommodity> priceCommodity = _context.PriceCommodity.ToList();
            var role = _context.Role.Where(y=> y.Id.Equals(Id)).Select(x => new { commodityDate = x.FullName, x.Name, x.ShortName, x.Module }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(role, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Role: {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TicketingReport")]
        public async Task<IActionResult> TicketingReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var ticketing = _context.Ticketing.Where(y => y.timeIn.Value.Year.Equals(Year) && y.timeIn.Value.Month.Equals(Month) && y.timeOut != null).Select(y => new { TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), ControlNumber = y.controlNumber, PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, Remarks = y.remarks, DriversName = y.driverName, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(ticketing, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Ticketing {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TicketingReportDate")]
        public async Task<IActionResult> TicketingReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.Ticketing.Where(x => x.timeOut != null).Select(y => new { TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), ControlNumber = y.controlNumber, PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, Remarks = y.remarks, DriversName = y.driverName, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk }).ToList();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet.Cells.LoadFromCollection(all, true);
                        package.Save();
                    }
                }
                
            }
            else if (Date == 1)
            {
                var currentDate = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today && x.timeOut != null).Select(y => new { TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), ControlNumber = y.controlNumber, PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, Remarks = y.remarks, DriversName = y.driverName, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-7) && x.timeOut != null).Select(y => new { TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), ControlNumber = y.controlNumber, PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, Remarks = y.remarks, DriversName = y.driverName, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-31) && x.timeOut != null).Select(y => new { TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), ControlNumber = y.controlNumber, PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, Remarks = y.remarks, DriversName = y.driverName, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-365) && x.timeOut != null).Select(y => new { TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), ControlNumber = y.controlNumber, PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, Remarks = y.remarks, DriversName = y.driverName, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }
            
            stream.Position = 0;
            string excelName = $"Ticketing {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalReport")]
        public async Task<IActionResult> TotalReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var total = _context.Total.Where(x => x.date.Year.Equals(Year) && x.date.Month.Equals(Month)).Select(x => new { Date = x.date.ToString("MMMM dd, yyyy / hh:mm tt"), Origin = x.origin, Amount = x.amount }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(total, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Total {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalReportDate")]
        public async Task<IActionResult> TotalReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.Total.Select(x => new { Date = x.date.ToString("MMMM dd, yyyy / hh:mm tt"), Origin = x.origin, Amount = x.amount }).ToList();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet.Cells.LoadFromCollection(all, true);
                        package.Save();
                    }
                }

            }
            else if (Date == 1)
            {
                var currentDate = _context.Total.Where(x => x.date >= DateTime.Today).Select(x => new { Date = x.date.ToString("MMMM dd, yyyy / hh:mm tt"), Origin = x.origin, Amount = x.amount }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.Total.Where(x => x.date >= DateTime.Today.AddDays(-7)).Select(x => new { Date = x.date.ToString("MMMM dd, yyyy / hh:mm tt"), Origin = x.origin, Amount = x.amount }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.Total.Where(x => x.date >= DateTime.Today.AddDays(-31)).Select(x => new { Date = x.date.ToString("MMMM dd, yyyy / hh:mm tt"), Origin = x.origin, Amount = x.amount }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.Total.Where(x => x.date >= DateTime.Today.AddDays(-365)).Select(x => new { Date = x.date.ToString("MMMM dd, yyyy / hh:mm tt"), Origin = x.origin, Amount = x.amount }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Total {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("StallLeasersReport")]
        public async Task<IActionResult> StallLeasersReport()
        {
            // query data from database  
            await Task.Yield();

            var stallLease = _context.StallLease.Where(x => x.EndDate >= DateTime.Now).Select(x => new { PlateNumber1 = x.PlateNumber1, PlateNumber2 = x.PlateNumber2, Remarks = x.Remarks, Amount = x.Amount }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(stallLease, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Stall Leasers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TradersReport")]
        public async Task<IActionResult> TradersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var tradersTruck = _context.TradersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(tradersTruck, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Traders truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TradersReportDate")]
        public async Task<IActionResult> TradersReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                //var all = _context.TradersTruck.Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                var all = _context.TradersTruck.Where(x => x.DateInspected != null).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Traders truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("FarmersReport")]
        public async Task<IActionResult> FarmersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var farmersTruck = _context.FarmersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province ,Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(farmersTruck, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Farmers truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("FarmersReportDate")]
        public async Task<IActionResult> FarmersReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.FarmersTruck.Where(x => x.DateInspected != null).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Farmers truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("ShortTripReport")]
        public async Task<IActionResult> ShortTripReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var shortTrip = _context.ShortTrip.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, Commodity = x.Commodity, EstimatedVolume = x.EstimatedVolume, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(shortTrip, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Short trip {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("ShortTripReportDate")]
        public async Task<IActionResult> ShortTripReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.ShortTrip.Where(x => x.DateInspected != null).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, Commodity = x.Commodity, EstimatedVolume = x.EstimatedVolume, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.ShortTrip.Where(x => x.DateInspected >= DateTime.Today).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, Commodity = x.Commodity, EstimatedVolume = x.EstimatedVolume, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.ShortTrip.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, Commodity = x.Commodity, EstimatedVolume = x.EstimatedVolume, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.ShortTrip.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, Commodity = x.Commodity, EstimatedVolume = x.EstimatedVolume, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.ShortTrip.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365)).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, Commodity = x.Commodity, EstimatedVolume = x.EstimatedVolume, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Short trip {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("InterTradingReport")]
        public async Task<IActionResult> InterTradingReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var interTrading = _context.InterTrading.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(interTrading, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Inter trading {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("InterTradingReportDate")]
        public async Task<IActionResult> InterTradingReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.InterTrading.Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.InterTrading.Where(x => x.Date >= DateTime.Today).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Inter trading {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("CarrotFacilityReport")]
        public async Task<IActionResult> CarrotFacilityReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var carrotFacility = _context.CarrotFacility.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(carrotFacility, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Carrot facility {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("CarrotFacilityReportDate")]
        public async Task<IActionResult> CarrotFacilityReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.CarrotFacility.Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Carrot facility {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("AccreditedInterTradersReport")]
        public async Task<IActionResult> AccreditedInterTradersReport()
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

                var intertraders = _context.AccreditedInterTraders.Select(x => new { DateOfApplication = x.DateOfApplication.ToString("MMMM dd, yyyy / hh:mm tt"), Name = x.Name, ReferenceNumber = x.ReferenceNumber, NameOfAssociation = x.NameOfAssociation, NameOfSpouse = x.NameOfSpouse, ContactNumber = x.ContactNumber, Barangay = x.Barangay, Municipality = x.Municipality, Province = x.Province, BusinessPermit = x.BusinessPermit, Tin = x.Tin, IdNumber = x.IdNumber, Counter = x.Counter, Destination = x.Destination, PresentAddress = x.PresentAddress }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(intertraders, true);
                    package.Save();
                }

            stream.Position = 0;
            string excelName = $"Accreditation inter-traders {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("AccreditedPackersAndPortersReport")]
        public async Task<IActionResult> AccreditedPackersAndPortersReport(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

                var packersAndPorters = _context.AccreditedPackersAndPorters.Select(x => new { DateOfApplication = x.DateOfApplication.ToString("MMMM dd, yyyy / hh:mm tt"), Name = x.Name, Nickname = x.NickName, NameOfAssociation = x.NameOfAssociation, NameOfSpouse = x.NameOfSpouse, Birthdate = x.BirthDate, ContactNumber = x.ContactNumber, Barangay = x.Barangay, Municipality = x.Municipality, Province = x.Province, IdNumber = x.IdNumber, Requirements = x.Requirements, ProvincialAddress = x.ProvincialAddress, PresentAddress = x.PresentAddress, PackerOrPorter = x.PackerOrPorter }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(packersAndPorters, true);
                    package.Save();
                }

            stream.Position = 0;
            string excelName = $"Accredited packers and porters {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("AccreditedBuyersReport")]
        public async Task<IActionResult> AccreditedBuyersReport(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

                var buyers = _context.AccreditedBuyers.Select(x => new { BusinessName = x.BusinessName, BusinessAddress = x.BusinessAddress, NameOfSpouse = x.NameOfSpouse, Birthdate = x.BirthDate, ContactNumber = x.ContactNumber, VehiclePlateNumber = x.VehiclePlateNumber, Barangay = x.Barangay, Muncipality = x.Municipality, Province = x.Province, PresentAddress = x.PresentAddress, ProductDestination = x.ProductDestination, Tin = x.Tin }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(buyers, true);
                    package.Save();
                }

            stream.Position = 0;
            string excelName = $"Accredited buyers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("AccreditedMarketFacilitatorsReport")]
        public async Task<IActionResult> AccreditedMarketFacilitatorsReport(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            var marketFacilitators = _context.AccreditedMarketFacilitators.Select(x => new { DateOfApplication = x.DateOfApplication.ToString("MMMM dd, yyyy / hh:mm tt"), Name = x.Name, Nickname = x.NickName, NameOfSpouse = x.NameOfSpouse, NameOfAssociation = x.NameOfAssociation, Birthdate = x.BirthDate, ContactNumber = x.ContactNumber, BusinessName = x.BusinessName, BusinessAddress = x.BusinessAddress, ReferenceNumber = x.ReferenceNumber, Barangay = x.Barangay, Municipality = x.Municipality, Province = x.Province, IdNumber = x.IdNumber, MajorCommodity = x.MajorCommodity, PresentAddress = x.PresentAddress, Tin = x.Tin }).ToList();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(marketFacilitators, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Accredited market facilitators {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("AccreditedIndividualFarmersReport")]
        public async Task<IActionResult> AccreditedIndividualFarmersReport(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            var individualFarmers = _context.AccreditedIndividualFarmers.Select(x => new { DateOfApplication = x.DateOfApplication.ToString("MMMM dd, yyyy / hh:mm tt"), Name = x.Name, PlateNumber = x.PlateNumber, SpouseName = x.SpouseName, ReferenceNumber = x.ReferenceNumber, Association = x.Association, ContactNumber = x.ContactNumber, Birthdate = x.BirthDate, MajorCrops = x.MajorCrops, EstimatedProduce = x.EstimatedProduce, EstimatedTotalLandAre = x.EstimatedTotalLandArea, Sitio = x.Sitio, Barangay = x.Barangay, Municipality = x.Municipality, Province = x.Province, Counter = x.Counter, IdNumber = x.IdNumber, Harvesting = x.Harvesting, Planting = x.Planting }).ToList();
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(individualFarmers, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Accredited individual farmers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


        [HttpGet("SecurityReport")]
        public async Task<IActionResult> SecurityReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var security = _context.SecurityInspectionReport.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(security, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Security {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("SecurityReportDate")]
        public async Task<IActionResult> SecurityReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.SecurityInspectionReport.Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Security {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("RepairReport")]
        public async Task<IActionResult> RepairReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var repair = _context.Repair.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(repair, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Repair {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("RepairReportDate")]
        public async Task<IActionResult> RepairReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.Repair.Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.Repair.Where(x => x.Date >= DateTime.Today).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Repair {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("PriceCommodityReport")]
        public async Task<IActionResult> PriceCommodityReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var priceCommodity = _context.PriceCommodity.Where(x => x.commodityDate.Year.Equals(Year) && x.commodityDate.Month.Equals(Month)).Select(x => new { Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, Price = x.priceRange }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(priceCommodity, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"PriceCommodity {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("PriceCommodityReportDate")]
        public async Task<IActionResult> PriceCommodityReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.PriceCommodity.Select(x => new { Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, Price = x.priceRange }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today).Select(x => new { Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, Price = x.priceRange }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-7)).Select(x => new { Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, Price = x.priceRange }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-31)).Select(x => new { Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, Price = x.priceRange }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-365)).Select(x => new { Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, Price = x.priceRange }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"PriceCommodity {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("SettingsReportRole")]
        public async Task<IActionResult> SettingsReportDate(int Role)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Role == 1000000)
            {
                var all = _context.ApplicationUser.Where(x => x.RoleId != "Default").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Role == 1)
            {
                var gsu = _context.ApplicationUser.Where(x => x.RoleId == "GSU").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(gsu, true);
                    package.Save();
                }
            }
            else if (Role == 2)
            {
                var idesu = _context.ApplicationUser.Where(x => x.RoleId == "IDESU").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(idesu, true);
                    package.Save();
                }
            }
            else if (Role == 3)
            {
                var ictmisu = _context.ApplicationUser.Where(x => x.RoleId == "ICT-MISU").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(ictmisu, true);
                    package.Save();
                }
            }
            else if (Role == 4)
            {
                var tod = _context.ApplicationUser.Where(x => x.RoleId == "TOD").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(tod, true);
                    package.Save();
                }
            }
            else if (Role == 5)
            {
                var fd = _context.ApplicationUser.Where(x => x.RoleId == "FD").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(fd, true);
                    package.Save();
                }
            }
            else if (Role == 6)
            {
                var aamd = _context.ApplicationUser.Where(x => x.RoleId == "AAMD").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(aamd, true);
                    package.Save();
                }
            }
            else if (Role == 7)
            {
                var pru = _context.ApplicationUser.Where(x => x.RoleId == "PRU").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(pru, true);
                    package.Save();
                }
            }
            else if (Role == 8)
            {
                var hrmu = _context.ApplicationUser.Where(x => x.RoleId == "HRMU").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(hrmu, true);
                    package.Save();
                }
            }
            else if (Role == 9)
            {
                var ad = _context.ApplicationUser.Where(x => x.RoleId == "AD").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(ad, true);
                    package.Save();
                }
            }
            else if (Role == 10)
            {
                var sd = _context.ApplicationUser.Where(x => x.RoleId == "SD").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(sd, true);
                    package.Save();
                }
            }
            else if (Role == 11)
            {
                var coo = _context.ApplicationUser.Where(x => x.RoleId == "COO").Select(x => new { FullName = x.FullName, EmailAddress = x.Email, PhoneNumber = x.PhoneNumber, Unit = x.RoleId }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(coo, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Employees {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

    }
}