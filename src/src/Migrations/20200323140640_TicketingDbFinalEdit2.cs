using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TicketingDbFinalEdit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "GatePass");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "GatePass",
                newName: "DriverName");

            migrationBuilder.AddColumn<string>(
                name: "driverName",
                table: "Ticketing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "driverName",
                table: "Ticketing");

            migrationBuilder.RenameColumn(
                name: "DriverName",
                table: "GatePass",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "GatePass",
                nullable: true);
        }
    }
}
