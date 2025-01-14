﻿using Microsoft.AspNetCore.Http;
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

            var ticketing = _context.Ticketing.Where(y => y.timeIn.Value.Year.Equals(Year) && y.timeIn.Value.Month.Equals(Month) && y.timeOut != null).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();

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
        public async Task<IActionResult> TicketingReportDate(int Date, string Type)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000 && Type == "Finished")
            {
                {
                    var all = _context.Ticketing.Where(x => x.Transaction == "Finished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet.Cells.LoadFromCollection(all, true);
                        package.Save();
                    }
                }
            }
            else if (Date == 1000000 && Type == "Unfinished")
            {
                {
                    var all = _context.Ticketing.Where(x => x.Transaction == "Unfinished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet.Cells.LoadFromCollection(all, true);
                        package.Save();
                    }
                }
            }
            else if (Date == 1 && Type == "Finished")
            {
                var currentDate = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today && x.Transaction == "Finished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 1 && Type == "Unfinished")
            {
                var currentDate = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today && x.Transaction == "Unfinished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7 && Type == "Finished")
            {
                var lastWeek = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-7) && x.Transaction == "Finished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 7 && Type == "Unfinished")
            {
                var lastWeek = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-7) && x.Transaction == "Unfinished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31 && Type == "Finished")
            {
                var lastMonth = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-31) && x.Transaction == "Finished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 31 && Type == "Unfinished")
            {
                var lastMonth = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-31) && x.Transaction == "Unfinished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365 && Type == "Finished")
            {
                var lastYear = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-365) && x.Transaction == "Finished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }
            else if (Date == 365 && Type == "Unfinished")
            {
                var lastYear = _context.Ticketing.Where(x => x.timeIn >= DateTime.Today.AddDays(-365) && x.Transaction == "Unfinished").OrderByDescending(x => x.timeIn).Select(y => new { Name = y.driverName, ContactNumber = y.ContactNumber, Address = y.Address, Temperature = y.Temperature, TimeIn = y.timeIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), TimeOut = y.timeOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.plateNumber, TypeOfTransaction = y.typeOfTransaction, TypeOfCar = y.typeOfCar, DriversName = y.driverName, ParkingNumber = y.parkingNumber, Accreditation = y.accreditation, Remarks = y.remarks, Count = y.count, Amount = y.amount, IssuingClerk = y.issuingClerk, ReceivingClerk = y.receivingClerk, ControlNumber = y.controlNumber }).ToList();
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


        [HttpGet("DailyBuyersReport")]
        public async Task<IActionResult> DailyBuyersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var buyers = _context.DailyBuyers.Where(y => y.Date.Year.Equals(Year) && y.Date.Month.Equals(Month)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(buyers, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"DailyBuyers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("DailyBuyersReportDate")]
        public async Task<IActionResult> DailyBuyersReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.DailyBuyers.Where(x => x.Date != null).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
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
                var currentDate = _context.DailyBuyers.Where(x => x.Date >= DateTime.Today).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.DailyBuyers.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.DailyBuyers.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.DailyBuyers.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"DailyBuyers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("DailyFarmersReport")]
        public async Task<IActionResult> DailyFarmersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var farmers = _context.DailyFarmers.Where(y => y.Date.Year.Equals(Year) && y.Date.Month.Equals(Month)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(farmers, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"DailyFarmers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("DailyFarmersReportDate")]
        public async Task<IActionResult> DailyFarmersReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.DailyFarmers.Where(x => x.Date != null).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
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
                var currentDate = _context.DailyFarmers.Where(x => x.Date >= DateTime.Today).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.DailyFarmers.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.DailyFarmers.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.DailyFarmers.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"DailyFarmers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("DailyFacilitatorsReport")]
        public async Task<IActionResult> DailyFacilitatorsReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var facilitators = _context.DailyFacilitators.Where(y => y.Date.Year.Equals(Year) && y.Date.Month.Equals(Month)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(facilitators, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"DailyFacilitators {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("DailyFacilitatorsReportDate")]
        public async Task<IActionResult> DailyFacilitatorsReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.DailyFacilitators.Where(x => x.Date != null).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
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
                var currentDate = _context.DailyFacilitators.Where(x => x.Date >= DateTime.Today).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.DailyFacilitators.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.DailyFacilitators.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.DailyFacilitators.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = y.PlateNumber, Count = y.Count }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"DailyFacilitators {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalBuyersReport")]
        public async Task<IActionResult> TotalBuyersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var buyers = _context.TotalBuyers.Where(y => y.Date.Year.Equals(Year) && y.Date.Month.Equals(Month)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(buyers, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"TotalBuyers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalBuyersReportDate")]
        public async Task<IActionResult> TotalBuyersReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.TotalBuyers.Where(x => x.Date != null).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
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
                var currentDate = _context.TotalBuyers.Where(x => x.Date >= DateTime.Today).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.TotalBuyers.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.TotalBuyers.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.TotalBuyers.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"TotalBuyers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalFarmersReport")]
        public async Task<IActionResult> TotalFarmersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var farmers = _context.TotalFarmers.Where(y => y.Date.Year.Equals(Year) && y.Date.Month.Equals(Month)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(farmers, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"TotalFarmers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalFarmersReportDate")]
        public async Task<IActionResult> TotalFarmersReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.TotalFarmers.Where(x => x.Date != null).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
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
                var currentDate = _context.TotalFarmers.Where(x => x.Date >= DateTime.Today).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.TotalFarmers.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.TotalFarmers.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.TotalFarmers.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"TotalFarmers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalFacilitatorsReport")]
        public async Task<IActionResult> TotalFacilitatorsReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var facilitators = _context.TotalFacilitators.Where(y => y.Date.Year.Equals(Year) && y.Date.Month.Equals(Month)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(facilitators, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"TotalFacilitators {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("TotalFacilitatorsReportDate")]
        public async Task<IActionResult> TotalFacilitatorsReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.TotalFacilitators.Where(x => x.Date != null).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
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
                var currentDate = _context.TotalFacilitators.Where(x => x.Date >= DateTime.Today).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.TotalFacilitators.Where(x => x.Date >= DateTime.Today.AddDays(-7)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.TotalFacilitators.Where(x => x.Date >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.TotalFacilitators.Where(x => x.Date >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.Date.ToString("MMMM dd, yyyy"), Total = y.Total }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"TotalFacilitators {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("ParkingNumberReportDate")]
        public async Task<IActionResult> ParkingNumberReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.ParkingNumbers.Select(y => new { ParkingNumber = y.Name, Occupied = y.Selected }).ToList();
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
                var occupied = _context.ParkingNumbers.Where(x => x.Selected == true).Select(y => new { ParkingNumber = y.Name, Occupied = y.Selected }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(occupied, true);
                    package.Save();
                }
            }

            else if (Date == 7)
            {
                var vacant = _context.ParkingNumbers.Where(x => x.Selected == false).Select(y => new { ParkingNumber = y.Name, Occupied = y.Selected }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(vacant, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"ParkingNumbers {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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

        [HttpGet("TradersWithParam")]
        public async Task<IActionResult> TradersWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var tradersTruck = _context.TradersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(tradersTruck, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special traders truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.TradersTruck.Where(x => x.DateInspected != null).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
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

        [HttpGet("TradersWithParam2")]
        public async Task<IActionResult> TradersWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                //var all = _context.TradersTruck.Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                var all = _context.TradersTruck.Where(x => x.DateInspected != null && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.TradersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, TradersName = x.TraderName, EstimatedVolume = x.EstimatedVolume, Destination = x.Destination, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special traders truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("FarmersReport")]
        public async Task<IActionResult> FarmersReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var farmersTruck = _context.FarmersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province ,Inspector = x.Inspector }).ToList();

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

        [HttpGet("FarmersWithParam")]
        public async Task<IActionResult> FarmersWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var farmersTruck = _context.FarmersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(farmersTruck, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special farmers truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.FarmersTruck.Where(x => x.DateInspected != null).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
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

        [HttpGet("FarmersWithParam2")]
        public async Task<IActionResult> FarmersWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.FarmersTruck.Where(x => x.DateInspected != null && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365) && x.Inspector == Inspector).OrderByDescending(x => x.DateInspected).Select(x => new { DateInspected = x.DateInspected.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, FarmersName = x.FarmersName, Organization = x.Organization, Volume = x.Volume, Commodity = x.Commodity, StallNumber = x.StallNumber, Barangay = x.Barangay, Province = x.Province, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special farmers truck {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("ShortTripReport")]
        public async Task<IActionResult> ShortTripReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var shortTrip = _context.ShortTrip.Where(x => x.DateInspectedOut.Value.Year.Equals(Year) && x.DateInspectedOut.Value.Month.Equals(Month)).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();

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

        [HttpGet("ShortTripWithParam")]
        public async Task<IActionResult> ShortTripWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var shortTrip = _context.ShortTrip.Where(x => x.DateInspectedOut.Value.Year.Equals(Year) && x.DateInspectedOut.Value.Month.Equals(Month) && x.InspectorIn == Inspector || x.InspectorOut == Inspector).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(shortTrip, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special short trip {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.ShortTrip.Where(x => x.DateInspectedOut != null).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
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

        [HttpGet("ShortTripWithParam2")]
        public async Task<IActionResult> ShortTripWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.ShortTrip.Where(x => x.DateInspectedOut != null && x.InspectorIn == Inspector || x.InspectorOut == Inspector).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today && x.InspectorIn == Inspector || x.InspectorOut == Inspector).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today.AddDays(-7) && x.InspectorIn == Inspector || x.InspectorOut == Inspector).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today.AddDays(-31) && x.InspectorIn == Inspector || x.InspectorOut == Inspector).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.ShortTrip.Where(x => x.DateInspectedOut >= DateTime.Today.AddDays(-365) && x.InspectorIn == Inspector || x.InspectorOut == Inspector).OrderByDescending(x => x.DateInspectedOut).Select(x => new { DateInspectedIn = x.DateInspectedIn.Value.ToString("MMMM dd, yyyy / hh:mm tt"), DateInspectedOut = x.DateInspectedOut.Value.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, CommodityIn = x.CommodityIn, EstimatedVolumeIn = x.EstimatedVolumeIn, InspectorIn = x.InspectorIn, CommodityOut = x.CommodityOut, EstimatedVolumeOut = x.EstimatedVolumeOut, InspectorOut = x.InspectorOut }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special short trip {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("InterTradingReport")]
        public async Task<IActionResult> InterTradingReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var interTrading = _context.InterTrading.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();

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

        [HttpGet("InterTradingWithParam")]
        public async Task<IActionResult> InterTradingWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var interTrading = _context.InterTrading.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(interTrading, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special inter trading {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.InterTrading.OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.InterTrading.Where(x => x.Date >= DateTime.Today).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
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

        [HttpGet("InterTradingWithParam2")]
        public async Task<IActionResult> InterTradingWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.InterTrading.Where(x => x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.InterTrading.Where(x => x.Date >= DateTime.Today && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-7) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-31) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.InterTrading.Where(x => x.Date >= DateTime.Today.AddDays(-365) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, FarmersName = x.FarmerName, FarmersOrganization = x.FarmersOrganization, Commodity = x.Commodity, Volume = x.Volume, ProductionAre = x.ProductionArea, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special inter trading {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("CarrotFacilityReport")]
        public async Task<IActionResult> CarrotFacilityReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var carrotFacility = _context.CarrotFacility.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();

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

        [HttpGet("CarrotFacilityWithParam")]
        public async Task<IActionResult> CarrotFacilityWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var carrotFacility = _context.CarrotFacility.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(carrotFacility, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special carrot facility {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.CarrotFacility.OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
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

        [HttpGet("CarrotFacilityWithParam2")]
        public async Task<IActionResult> CarrotFacilityWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.CarrotFacility.Where(x => x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-7) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-31) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.CarrotFacility.Where(x => x.Date >= DateTime.Today.AddDays(-365) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { DateInspected = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Code = x.Code, Commodity = x.Commodity, Volume = x.Volume, Destination = x.Destination, StallNumber = x.StallNumber, Facilitator = x.Facilitator, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special carrot facility {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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

            var security = _context.SecurityInspectionReport.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();

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

        [HttpGet("SecurityWithParam")]
        public async Task<IActionResult> SecurityWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var security = _context.SecurityInspectionReport.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(security, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special security {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.SecurityInspectionReport.OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
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

        [HttpGet("SecurityWithParam2")]
        public async Task<IActionResult> SecurityWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.SecurityInspectionReport.Where(x => x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-7) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-31) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.SecurityInspectionReport.Where(x => x.Date >= DateTime.Today.AddDays(-365) && x.Inspector == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), Location = x.Location, Remarks = x.Remarks, Action = x.Action, Inspector = x.Inspector }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special security {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("RepairReport")]
        public async Task<IActionResult> RepairReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var repair = _context.Repair.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();

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

        [HttpGet("RepairWithParam")]
        public async Task<IActionResult> RepairWithParam(int Year, int Month, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var repair = _context.Repair.Where(x => x.Date.Year.Equals(Year) && x.Date.Month.Equals(Month) && x.RequesterName == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(repair, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Special repair {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.Repair.OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.Repair.Where(x => x.Date >= DateTime.Today).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-7)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-31)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-365)).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
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

        [HttpGet("RepairWithParam2")]
        public async Task<IActionResult> RepairWithParam2(int Date, string Inspector)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.Repair.Where(x => x.RequesterName == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.Repair.Where(x => x.Date >= DateTime.Today && x.RequesterName == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-7) && x.RequesterName == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-31) && x.RequesterName == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.Repair.Where(x => x.Date >= DateTime.Today.AddDays(-365) && x.RequesterName == Inspector).OrderByDescending(x => x.Date).Select(x => new { Date = x.Date.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, RequestNumber = x.RequestNumber, DriversName = x.DriverName, Location = x.Location, Destination = x.Destination, RepairDetails = x.RepairDetails, Remarks = x.Remarks, RequesterName = x.RequesterName }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Special repair {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("PriceCommodityReport")]
        public async Task<IActionResult> PriceCommodityReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var priceCommodity = _context.PriceCommodity.Where(x => x.commodityDate.Year.Equals(Year) && x.commodityDate.Month.Equals(Month)).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();

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

        [HttpGet("PriceWithParamReport")]
        public async Task<IActionResult> PriceWithParamReport(int Year, int Month, string Commodity, string Variety)
        {
            // query data from database  
            await Task.Yield();

            var priceCommodity = _context.PriceCommodity.Where(x => x.commodityDate.Year.Equals(Year) && x.commodityDate.Month.Equals(Month) && x.commodity == Commodity && x.classVariety == Variety).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM yyyy"), Commodity = x.commodity, ClassVariety = x.classVariety }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(priceCommodity, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"SelectedPriceCommodity {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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
                var all = _context.PriceCommodity.Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-7)).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-31)).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-365)).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
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

        [HttpGet("PriceWithParamReport2")]
        public async Task<IActionResult> PriceWithParamReport2(int Date, string Commodity, string Variety)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.PriceCommodity.Where(x => x.commodity == Commodity && x.classVariety == Variety).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today && x.commodity == Commodity && x.classVariety == Variety).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-7) && x.commodity == Commodity && x.classVariety == Variety).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-31) && x.commodity == Commodity && x.classVariety == Variety).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.PriceCommodity.Where(x => x.commodityDate >= DateTime.Today.AddDays(-365) && x.commodity == Commodity && x.classVariety == Variety).OrderByDescending(x => x.commodityDate).Select(x => new { AverageLow = x.averageLow, AverageHigh = x.averageHigh, Date = x.commodityDate.ToString("MMMM dd, yyyy / hh:mm tt"), Commodity = x.commodity, ClassVariety = x.classVariety, CommodityRemarks = x.commodityRemarks, PriceLow = x.priceLow, PriceHigh = x.priceHigh }).ToList();
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

        [HttpGet("CommodityVolumeReport")]
        public async Task<IActionResult> CommodityVolumeReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var priceCommodity = _context.FarmersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month)).OrderByDescending(x => x.DateInspected).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(priceCommodity, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Commodity Volume {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("CommodityWithParamReport")]
        public async Task<IActionResult> CommodityWithParamReport(int Year, int Month, string Commodity)
        {
            // query data from database  
            await Task.Yield();

            var priceCommodity = _context.FarmersTruck.Where(x => x.DateInspected.Value.Year.Equals(Year) && x.DateInspected.Value.Month.Equals(Month) && x.Commodity == Commodity).OrderByDescending(x => x.DateInspected).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(priceCommodity, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Selected Commodity Volume {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("CommodityVolumeReportDate")]
        public async Task<IActionResult> CommodityVolumeReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.FarmersTruck.Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7)).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31)).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365)).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Commodity Volume {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("CommodityWithParamReport2")]
        public async Task<IActionResult> CommodityWithParamReport2(int Date, string Commodity)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                var all = _context.FarmersTruck.Where(x => x.Commodity == Commodity).OrderByDescending(x => x.DateInspected).Select(x => new { Date = x.DateInspected.Value.ToString("MMMM yyyy"), Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(all, true);
                    package.Save();
                }
            }
            else if (Date == 1)
            {
                var currentDate = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today && x.Commodity == Commodity).OrderByDescending(x => x.DateInspected.Value.ToString("MMMM yyyy")).Select(x => new { Date = x.DateInspected, Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(currentDate, true);
                    package.Save();
                }
            }
            else if (Date == 7)
            {
                var lastWeek = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-7) && x.Commodity == Commodity).OrderByDescending(x => x.DateInspected.Value.ToString("MMMM yyyy")).Select(x => new { Date = x.DateInspected, Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastWeek, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-31) && x.Commodity == Commodity).OrderByDescending(x => x.DateInspected.Value.ToString("MMMM yyyy")).Select(x => new { Date = x.DateInspected, Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.FarmersTruck.Where(x => x.DateInspected >= DateTime.Today.AddDays(-365) && x.Commodity == Commodity).OrderByDescending(x => x.DateInspected.Value.ToString("MMMM yyyy")).Select(x => new { Date = x.DateInspected, Commodity = x.Commodity, Volume = x.Volume }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Selected Commodity Volume {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

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




        [HttpGet("DeletedDatas")]
        public async Task<IActionResult> DeletedDatas()
        {
            // query data from database  
            await Task.Yield();

            var deleted = _context.DeletedDatas.Select(x => new { Id = x.Id ,DateDeleted = x.DateDeleted.ToString("MMMM dd, yyyy / hh:mm tt"), PlateNumber = x.PlateNumber, From = x.Origin, Name = x.Name, DeletedBy = x.DeletedBy, Remarks = x.Remarks, Amount = x.Amount }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(deleted, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Deleted datas {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        // DELETE: api/Security/DeleteSecurityInspectionReport
        [HttpDelete("ResetDatabase")]
        public async Task<IActionResult> ResetDatabase([FromRoute] int id)
        {
            var listTicketing = _context.Ticketing.Where(x => x.timeOut != null).ToList();
            _context.RemoveRange(listTicketing);

            var listTradersTruck = _context.TradersTruck.Where(x => x.DateInspected != null).ToList();
            _context.RemoveRange(listTradersTruck);

            var listFarmersTruck = _context.FarmersTruck.Where(x => x.DateInspected != null).ToList();
            _context.RemoveRange(listFarmersTruck);

            var listShortTrip = _context.ShortTrip.Where(x => x.DateInspectedOut != null).ToList();
            _context.RemoveRange(listShortTrip);

            var listIntertrading = _context.InterTrading.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listIntertrading);

            var listSecurity = _context.SecurityInspectionReport.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listSecurity);

            var listRepair = _context.Repair.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listRepair);

            var listPriceCommodities = _context.PriceCommodity.Where(x => x.commodityDate != null).ToList();
            _context.RemoveRange(listPriceCommodities);

            var listDeletedDatas = _context.DeletedDatas.Where(x => x.DateDeleted != null).ToList();
            _context.RemoveRange(listDeletedDatas);

            var listEditedDatas = _context.EditedDatas.Where(x => x.DateEdited != null).ToList();
            _context.RemoveRange(listEditedDatas);

            var listCarrotFacility = _context.CarrotFacility.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listCarrotFacility);

            var listCurrentTicket = _context.CurrentTicket.Where(x => x.plateNumber != null).ToList();
            _context.RemoveRange(listCurrentTicket);

            var listDailyBuyers = _context.DailyBuyers.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listDailyBuyers);

            var listDailyFacilitators = _context.DailyFacilitators.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listDailyFacilitators);

            var listDailyFarmers = _context.DailyFarmers.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listDailyFarmers);

            var listGatepass = _context.GatePass.Where(x => x.PlateNumber != null).ToList();
            _context.RemoveRange(listGatepass);

            var listPayParking = _context.PayParking.Where(x => x.PlateNumber != null).ToList();
            _context.RemoveRange(listPayParking);

            var listTotal = _context.Total.Where(x => x.date != null).ToList();
            _context.RemoveRange(listTotal);

            var listTotalBuyers = _context.TotalBuyers.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listTotalBuyers);

            var listTotalFacilitators = _context.TotalFacilitators.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listTotalFacilitators);

            var listTotalFarmers = _context.TotalFarmers.Where(x => x.Date != null).ToList();
            _context.RemoveRange(listTotalFarmers);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}