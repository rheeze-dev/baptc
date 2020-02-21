using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class databaseUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Ticket",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.CreateTable(
                name: "AbsenceRequest",
                columns: table => new
                {
                    absenceRequestId = table.Column<Guid>(nullable: false),
                    absenceType = table.Column<string>(maxLength: 300, nullable: false),
                    approvalStatus = table.Column<string>(maxLength: 300, nullable: true),
                    fillingDate = table.Column<DateTime>(maxLength: 300, nullable: false),
                    inclusiveDates = table.Column<DateTime>(maxLength: 300, nullable: false),
                    reasons = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    totalNumberOfDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbsenceRequest", x => x.absenceRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Accreditation",
                columns: table => new
                {
                    accreditationId = table.Column<Guid>(nullable: false),
                    address = table.Column<string>(nullable: true),
                    areaPlanted = table.Column<string>(maxLength: 300, nullable: true),
                    crops = table.Column<string>(maxLength: 300, nullable: true),
                    farmerName = table.Column<string>(maxLength: 300, nullable: true),
                    monthHarvested = table.Column<string>(nullable: true),
                    monthPlanted = table.Column<string>(nullable: true),
                    municipality = table.Column<string>(nullable: true),
                    plateNumber = table.Column<string>(maxLength: 300, nullable: false),
                    totalLandArea = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accreditation", x => x.accreditationId);
                });

            migrationBuilder.CreateTable(
                name: "Clerk",
                columns: table => new
                {
                    clerkId = table.Column<Guid>(nullable: false),
                    classification = table.Column<string>(nullable: true),
                    clerkDate = table.Column<DateTime>(maxLength: 300, nullable: false),
                    monthPaid = table.Column<string>(maxLength: 300, nullable: true),
                    orNumber = table.Column<string>(maxLength: 300, nullable: true),
                    payor = table.Column<string>(nullable: true),
                    totalAmount = table.Column<double>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clerk", x => x.clerkId);
                });

            migrationBuilder.CreateTable(
                name: "Compensatory",
                columns: table => new
                {
                    compensatoryId = table.Column<Guid>(nullable: false),
                    applicationDate = table.Column<DateTime>(maxLength: 300, nullable: false),
                    approvalStatus = table.Column<string>(nullable: true),
                    daysAvailable = table.Column<string>(nullable: true),
                    requestDate = table.Column<DateTime>(maxLength: 300, nullable: false),
                    supervisor = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compensatory", x => x.compensatoryId);
                });

            migrationBuilder.CreateTable(
                name: "DayOffAuthorization",
                columns: table => new
                {
                    dayOffAuthorizationId = table.Column<Guid>(nullable: false),
                    absenceId = table.Column<string>(nullable: true),
                    approveStatus = table.Column<string>(maxLength: 300, nullable: true),
                    expectedOutput = table.Column<string>(maxLength: 300, nullable: true),
                    remarks = table.Column<string>(maxLength: 300, nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    tasks = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayOffAuthorization", x => x.dayOffAuthorizationId);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employeeId = table.Column<Guid>(nullable: false),
                    designationOffice = table.Column<string>(maxLength: 300, nullable: true),
                    employeeName = table.Column<string>(maxLength: 300, nullable: false),
                    employmentDate = table.Column<DateTime>(nullable: false),
                    position = table.Column<string>(nullable: true),
                    totalAttendance = table.Column<string>(maxLength: 300, nullable: true),
                    userPassword = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.employeeId);
                });

            migrationBuilder.CreateTable(
                name: "Finance",
                columns: table => new
                {
                    financeId = table.Column<Guid>(nullable: false),
                    financeName = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance", x => x.financeId);
                });

            migrationBuilder.CreateTable(
                name: "Hr",
                columns: table => new
                {
                    hrId = table.Column<Guid>(nullable: false),
                    hrName = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hr", x => x.hrId);
                });

            migrationBuilder.CreateTable(
                name: "HrForm",
                columns: table => new
                {
                    HrFormId = table.Column<Guid>(nullable: false),
                    absence = table.Column<string>(maxLength: 300, nullable: true),
                    compensatory = table.Column<string>(maxLength: 300, nullable: false),
                    dayOffReport = table.Column<string>(nullable: true),
                    requestDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HrForm", x => x.HrFormId);
                });

            migrationBuilder.CreateTable(
                name: "Inspector",
                columns: table => new
                {
                    inspectorId = table.Column<Guid>(nullable: false),
                    controlId = table.Column<string>(nullable: true),
                    dateChecked = table.Column<DateTime>(maxLength: 300, nullable: false),
                    inspectorName = table.Column<string>(maxLength: 300, nullable: false),
                    typeOfTransaction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspector", x => x.inspectorId);
                });

            migrationBuilder.CreateTable(
                name: "PriceCommodity",
                columns: table => new
                {
                    priceCommodityId = table.Column<Guid>(nullable: false),
                    classVariety = table.Column<string>(nullable: true),
                    commodity = table.Column<string>(nullable: true),
                    commodityDate = table.Column<DateTime>(maxLength: 300, nullable: false),
                    commodityRemarks = table.Column<string>(maxLength: 300, nullable: true),
                    priceRange = table.Column<double>(maxLength: 300, nullable: false),
                    time = table.Column<DateTime>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceCommodity", x => x.priceCommodityId);
                });

            migrationBuilder.CreateTable(
                name: "Repair",
                columns: table => new
                {
                    repairId = table.Column<Guid>(nullable: false),
                    contactNumber = table.Column<string>(nullable: true),
                    destination = table.Column<string>(maxLength: 300, nullable: true),
                    driverName = table.Column<string>(nullable: true),
                    ownerName = table.Column<string>(maxLength: 300, nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    repairDetails = table.Column<string>(maxLength: 300, nullable: true),
                    repairTime = table.Column<DateTime>(nullable: false),
                    requestedName = table.Column<string>(nullable: true),
                    sideMarkings = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repair", x => x.repairId);
                });

            migrationBuilder.CreateTable(
                name: "Stalls",
                columns: table => new
                {
                    stallsId = table.Column<Guid>(nullable: false),
                    payment = table.Column<string>(maxLength: 300, nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    stallOwner = table.Column<string>(maxLength: 300, nullable: false),
                    transferRequest = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stalls", x => x.stallsId);
                });

            migrationBuilder.CreateTable(
                name: "Ticketing",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    gatePassDate = table.Column<string>(maxLength: 300, nullable: true),
                    plateNumber = table.Column<string>(nullable: true),
                    timeIn = table.Column<DateTime>(maxLength: 300, nullable: false),
                    timeOut = table.Column<DateTime>(maxLength: 300, nullable: false),
                    typeOfTransaction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticketing", x => x.ticketingId);
                });

            migrationBuilder.CreateTable(
                name: "Traders",
                columns: table => new
                {
                    tradersId = table.Column<Guid>(nullable: false),
                    address = table.Column<string>(maxLength: 300, nullable: true),
                    contactNumber = table.Column<string>(nullable: true),
                    stallId = table.Column<int>(nullable: false),
                    traderName = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traders", x => x.tradersId);
                });

            migrationBuilder.CreateTable(
                name: "Watchmen",
                columns: table => new
                {
                    watchmenId = table.Column<Guid>(nullable: false),
                    otherReports = table.Column<string>(nullable: true),
                    repairCheck = table.Column<string>(maxLength: 300, nullable: true),
                    watchmenName = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchmen", x => x.watchmenId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsenceRequest");

            migrationBuilder.DropTable(
                name: "Accreditation");

            migrationBuilder.DropTable(
                name: "Clerk");

            migrationBuilder.DropTable(
                name: "Compensatory");

            migrationBuilder.DropTable(
                name: "DayOffAuthorization");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Finance");

            migrationBuilder.DropTable(
                name: "Hr");

            migrationBuilder.DropTable(
                name: "HrForm");

            migrationBuilder.DropTable(
                name: "Inspector");

            migrationBuilder.DropTable(
                name: "PriceCommodity");

            migrationBuilder.DropTable(
                name: "Repair");

            migrationBuilder.DropTable(
                name: "Stalls");

            migrationBuilder.DropTable(
                name: "Ticketing");

            migrationBuilder.DropTable(
                name: "Traders");

            migrationBuilder.DropTable(
                name: "Watchmen");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Ticket",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
