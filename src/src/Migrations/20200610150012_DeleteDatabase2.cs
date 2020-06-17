using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class DeleteDatabase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParkingNumber",
                table: "PayParking",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParkingNumber",
                table: "GatePass",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingNumber",
                table: "PayParking");

            migrationBuilder.DropColumn(
                name: "ParkingNumber",
                table: "GatePass");
        }
    }
}
