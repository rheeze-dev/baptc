using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class PriceDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "average",
                table: "PriceCommodity");

            migrationBuilder.AddColumn<double>(
                name: "averageHigh",
                table: "PriceCommodity",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "averageLow",
                table: "PriceCommodity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "averageHigh",
                table: "PriceCommodity");

            migrationBuilder.DropColumn(
                name: "averageLow",
                table: "PriceCommodity");

            migrationBuilder.AddColumn<double>(
                name: "average",
                table: "PriceCommodity",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
