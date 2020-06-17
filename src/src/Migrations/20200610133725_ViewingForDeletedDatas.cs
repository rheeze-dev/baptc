using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class ViewingForDeletedDatas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParkingNumber",
                table: "TradersTruck",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParkingNumber",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParkingNumber",
                table: "FarmersTruck",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingNumber",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "ParkingNumber",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "ParkingNumber",
                table: "FarmersTruck");
        }
    }
}
