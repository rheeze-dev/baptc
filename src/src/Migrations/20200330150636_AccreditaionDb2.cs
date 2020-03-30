using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AccreditaionDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accredited");

            migrationBuilder.CreateTable(
                name: "AccreditedBuyers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    BusinessAddress = table.Column<string>(nullable: true),
                    BusinessName = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    Municipality = table.Column<string>(nullable: true),
                    NameOfSpouse = table.Column<string>(nullable: true),
                    PresentAddress = table.Column<string>(nullable: true),
                    ProductDestination = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    Tin = table.Column<int>(nullable: false),
                    VehiclePlateNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditedBuyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccreditedIndividualFarmers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Association = table.Column<string>(nullable: true),
                    Barangay = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: true),
                    Counter = table.Column<string>(nullable: true),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    EstimatedProduce = table.Column<int>(nullable: false),
                    EstimatedTotalLandArea = table.Column<string>(nullable: true),
                    Harvesting = table.Column<DateTime>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    LandAreaPerCrop = table.Column<string>(nullable: true),
                    MajorCrops = table.Column<string>(nullable: true),
                    Municipality = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Planting = table.Column<DateTime>(nullable: false),
                    PlateNumber = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    ReferenceNumber = table.Column<int>(nullable: false),
                    Sitio = table.Column<string>(nullable: true),
                    SpouseName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditedIndividualFarmers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccreditedInterTraders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: true),
                    BusinessPermit = table.Column<string>(nullable: true),
                    Column1 = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    Counter = table.Column<string>(nullable: true),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    IdNumber = table.Column<int>(nullable: false),
                    Municipality = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameOfAssociation = table.Column<string>(nullable: true),
                    NameOfSpouse = table.Column<string>(nullable: true),
                    PresentAddress = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    ReferenceNumber = table.Column<int>(nullable: false),
                    Tin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditedInterTraders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccreditedMarketFacilitators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    BusinessAddress = table.Column<string>(nullable: true),
                    BusinessName = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    MajorCommodity = table.Column<string>(nullable: true),
                    Municipality = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameOfAssociation = table.Column<string>(nullable: true),
                    NameOfSpouse = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    PresentAddress = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    ReferenceNumber = table.Column<int>(nullable: false),
                    Tin = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditedMarketFacilitators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccreditedPackersAndPorters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Barangay = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: true),
                    DateOfApplication = table.Column<DateTime>(nullable: false),
                    IdNumber = table.Column<int>(nullable: false),
                    Municipality = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NameOfAssociation = table.Column<string>(nullable: true),
                    NameOfSpouse = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    PackerOrPorter = table.Column<string>(nullable: true),
                    PresentAddress = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    ProvincialAddress = table.Column<string>(nullable: true),
                    Requirements = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccreditedPackersAndPorters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccreditedBuyers");

            migrationBuilder.DropTable(
                name: "AccreditedIndividualFarmers");

            migrationBuilder.DropTable(
                name: "AccreditedInterTraders");

            migrationBuilder.DropTable(
                name: "AccreditedMarketFacilitators");

            migrationBuilder.DropTable(
                name: "AccreditedPackersAndPorters");

            migrationBuilder.CreateTable(
                name: "Accredited",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accredited", x => x.Id);
                });
        }
    }
}
