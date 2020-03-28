using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TradingInspector6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Inspector",
                table: "TradersTruck",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inspector",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Inspector",
                table: "FarmersTruck",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inspector",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "Inspector",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "Inspector",
                table: "FarmersTruck");
        }
    }
}
