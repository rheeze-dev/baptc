using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedRemarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TradersTruck",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "InterTrading",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "FarmersTruck",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "CarrotFacility",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AccreditedPackersAndPorters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AccreditedMarketFacilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AccreditedInterTraders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AccreditedIndividualFarmers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "AccreditedBuyers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "InterTrading");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "FarmersTruck");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "CarrotFacility");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AccreditedPackersAndPorters");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AccreditedMarketFacilitators");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AccreditedInterTraders");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AccreditedIndividualFarmers");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "AccreditedBuyers");
        }
    }
}
