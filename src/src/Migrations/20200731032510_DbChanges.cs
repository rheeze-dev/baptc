using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class DbChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Role",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "ParkingNumbers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "GatePass",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StallNumber",
                table: "GatePass",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Commodities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifier",
                table: "Addresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "ParkingNumbers");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "StallNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Addresses");
        }
    }
}
