using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class PriceDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "priceRange",
                table: "PriceCommodity",
                newName: "priceLow");

            migrationBuilder.AddColumn<double>(
                name: "average",
                table: "PriceCommodity",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "priceHigh",
                table: "PriceCommodity",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "totalDays",
                table: "PriceCommodity",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average",
                table: "PriceCommodity");

            migrationBuilder.DropColumn(
                name: "priceHigh",
                table: "PriceCommodity");

            migrationBuilder.DropColumn(
                name: "totalDays",
                table: "PriceCommodity");

            migrationBuilder.RenameColumn(
                name: "priceLow",
                table: "PriceCommodity",
                newName: "priceRange");
        }
    }
}
