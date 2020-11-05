using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class EnteredByAccreditation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnteredBy",
                table: "AccreditedPackersAndPorters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnteredBy",
                table: "AccreditedMarketFacilitators",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnteredBy",
                table: "AccreditedInterTraders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnteredBy",
                table: "AccreditedIndividualFarmers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnteredBy",
                table: "AccreditedBuyers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnteredBy",
                table: "AccreditedPackersAndPorters");

            migrationBuilder.DropColumn(
                name: "EnteredBy",
                table: "AccreditedMarketFacilitators");

            migrationBuilder.DropColumn(
                name: "EnteredBy",
                table: "AccreditedInterTraders");

            migrationBuilder.DropColumn(
                name: "EnteredBy",
                table: "AccreditedIndividualFarmers");

            migrationBuilder.DropColumn(
                name: "EnteredBy",
                table: "AccreditedBuyers");
        }
    }
}
