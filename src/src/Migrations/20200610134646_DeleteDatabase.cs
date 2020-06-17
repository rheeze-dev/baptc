using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class DeleteDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeletedBuyers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: false),
                    BirthDate = table.Column<string>(nullable: false),
                    BusinessAddress = table.Column<string>(nullable: false),
                    BusinessName = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    Municipality = table.Column<string>(nullable: false),
                    NameOfSpouse = table.Column<string>(nullable: false),
                    PresentAddress = table.Column<string>(nullable: false),
                    ProductDestination = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    Tin = table.Column<int>(nullable: false),
                    VehiclePlateNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedBuyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedCarrotFacility",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccreditationChecker = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: true),
                    Commodity = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: false),
                    Facilitator = table.Column<string>(nullable: false),
                    Inspector = table.Column<string>(nullable: false),
                    StallNumber = table.Column<string>(nullable: false),
                    Volume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedCarrotFacility", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedFarmersTruck",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    AccreditationChecker = table.Column<string>(nullable: true),
                    Barangay = table.Column<string>(nullable: false),
                    Commodity = table.Column<string>(nullable: false),
                    DateInspected = table.Column<DateTime>(nullable: true),
                    FacilitatorsName = table.Column<string>(nullable: true),
                    FarmersName = table.Column<string>(nullable: false),
                    Inspector = table.Column<string>(nullable: true),
                    Municipality = table.Column<string>(nullable: true),
                    Organization = table.Column<string>(nullable: false),
                    ParkingNumber = table.Column<string>(nullable: true),
                    PlateNumber = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    StallNumber = table.Column<string>(nullable: false),
                    TimeIn = table.Column<DateTime>(nullable: false),
                    TimeOut = table.Column<DateTime>(nullable: true),
                    Volume = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedFarmersTruck", x => x.ticketingId);
                });

            migrationBuilder.CreateTable(
                name: "DeletedIndividualFarmers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Association = table.Column<string>(nullable: false),
                    Barangay = table.Column<string>(nullable: false),
                    BirthDate = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Counter = table.Column<string>(nullable: false),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    EstimatedProduce = table.Column<int>(nullable: false),
                    EstimatedTotalLandArea = table.Column<string>(nullable: false),
                    Harvesting = table.Column<string>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    LandAreaPerCrop = table.Column<string>(nullable: false),
                    MajorCrops = table.Column<string>(nullable: false),
                    Municipality = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Planting = table.Column<string>(nullable: false),
                    PlateNumber = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ReferenceNumber = table.Column<int>(nullable: false),
                    Sitio = table.Column<string>(nullable: true),
                    SpouseName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedIndividualFarmers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedInterTraders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: false),
                    BusinessPermit = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    Counter = table.Column<string>(nullable: false),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    Municipality = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NameOfAssociation = table.Column<string>(nullable: false),
                    NameOfSpouse = table.Column<string>(nullable: false),
                    PresentAddress = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ReferenceNumber = table.Column<int>(nullable: false),
                    Tin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedInterTraders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedInterTrading",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: true),
                    Commodity = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    FarmerName = table.Column<string>(nullable: false),
                    FarmersOrganization = table.Column<string>(nullable: false),
                    Inspector = table.Column<string>(nullable: false),
                    ProductionArea = table.Column<string>(nullable: false),
                    Volume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedInterTrading", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedMarketFacilitators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: false),
                    BirthDate = table.Column<string>(nullable: false),
                    BusinessAddress = table.Column<string>(nullable: false),
                    BusinessName = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    MajorCommodity = table.Column<string>(nullable: false),
                    Municipality = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NameOfAssociation = table.Column<string>(nullable: false),
                    NameOfSpouse = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: false),
                    PlateNumber = table.Column<string>(nullable: false),
                    PresentAddress = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ReferenceNumber = table.Column<int>(nullable: false),
                    Tin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedMarketFacilitators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedPackersAndPorters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: false),
                    BirthDate = table.Column<string>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: false),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    Municipality = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    NameOfAssociation = table.Column<string>(nullable: false),
                    NameOfSpouse = table.Column<string>(nullable: false),
                    NickName = table.Column<string>(nullable: false),
                    PackerOrPorter = table.Column<string>(nullable: false),
                    PresentAddress = table.Column<string>(nullable: false),
                    Province = table.Column<string>(nullable: false),
                    ProvincialAddress = table.Column<string>(nullable: false),
                    Requirements = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedPackersAndPorters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedPriceCommodity",
                columns: table => new
                {
                    priceCommodityId = table.Column<Guid>(nullable: false),
                    classVariety = table.Column<string>(nullable: false),
                    commodity = table.Column<string>(nullable: false),
                    commodityDate = table.Column<DateTime>(nullable: false),
                    commodityRemarks = table.Column<string>(nullable: true),
                    priceRange = table.Column<double>(nullable: false),
                    time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedPriceCommodity", x => x.priceCommodityId);
                });

            migrationBuilder.CreateTable(
                name: "DeletedRepair",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: false),
                    DriverName = table.Column<string>(nullable: false),
                    Location = table.Column<string>(nullable: false),
                    PlateNumber = table.Column<string>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    RepairDetails = table.Column<string>(nullable: false),
                    RequestNumber = table.Column<int>(nullable: true),
                    RequesterName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedRepair", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedSecurityInspectionReport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Inspector = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedSecurityInspectionReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeletedShortTrip",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    Commodity = table.Column<string>(nullable: false),
                    DateInspected = table.Column<DateTime>(nullable: true),
                    EstimatedVolume = table.Column<int>(nullable: true),
                    Inspector = table.Column<string>(nullable: true),
                    ParkingNumber = table.Column<string>(nullable: true),
                    PlateNumber = table.Column<string>(nullable: true),
                    TimeIn = table.Column<DateTime>(nullable: false),
                    TimeOut = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedShortTrip", x => x.ticketingId);
                });

            migrationBuilder.CreateTable(
                name: "DeletedStallLease",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    DriverName = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PlateNumber1 = table.Column<string>(nullable: true),
                    PlateNumber2 = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedStallLease", x => x.ticketingId);
                });

            migrationBuilder.CreateTable(
                name: "DeletedTicketing",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    accreditation = table.Column<string>(nullable: true),
                    amount = table.Column<int>(nullable: true),
                    controlNumber = table.Column<int>(nullable: true),
                    count = table.Column<int>(nullable: true),
                    driverName = table.Column<string>(nullable: true),
                    endDate = table.Column<DateTime>(nullable: true),
                    issuingClerk = table.Column<string>(nullable: true),
                    parkingNumber = table.Column<string>(nullable: true),
                    plateNumber = table.Column<string>(nullable: false),
                    receivingClerk = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    timeIn = table.Column<DateTime>(nullable: true),
                    timeOut = table.Column<DateTime>(nullable: true),
                    typeOfCar = table.Column<string>(nullable: true),
                    typeOfTransaction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedTicketing", x => x.ticketingId);
                });

            migrationBuilder.CreateTable(
                name: "DeletedTradersTruck",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    DateInspected = table.Column<DateTime>(nullable: true),
                    Destination = table.Column<string>(nullable: false),
                    EstimatedVolume = table.Column<int>(nullable: true),
                    Inspector = table.Column<string>(nullable: true),
                    ParkingNumber = table.Column<string>(nullable: true),
                    PlateNumber = table.Column<string>(nullable: true),
                    TimeIn = table.Column<DateTime>(nullable: false),
                    TimeOut = table.Column<DateTime>(nullable: true),
                    TraderName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedTradersTruck", x => x.ticketingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeletedBuyers");

            migrationBuilder.DropTable(
                name: "DeletedCarrotFacility");

            migrationBuilder.DropTable(
                name: "DeletedFarmersTruck");

            migrationBuilder.DropTable(
                name: "DeletedIndividualFarmers");

            migrationBuilder.DropTable(
                name: "DeletedInterTraders");

            migrationBuilder.DropTable(
                name: "DeletedInterTrading");

            migrationBuilder.DropTable(
                name: "DeletedMarketFacilitators");

            migrationBuilder.DropTable(
                name: "DeletedPackersAndPorters");

            migrationBuilder.DropTable(
                name: "DeletedPriceCommodity");

            migrationBuilder.DropTable(
                name: "DeletedRepair");

            migrationBuilder.DropTable(
                name: "DeletedSecurityInspectionReport");

            migrationBuilder.DropTable(
                name: "DeletedShortTrip");

            migrationBuilder.DropTable(
                name: "DeletedStallLease");

            migrationBuilder.DropTable(
                name: "DeletedTicketing");

            migrationBuilder.DropTable(
                name: "DeletedTradersTruck");
        }
    }
}
