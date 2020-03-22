using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class PayParkingDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayParking",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    DriverName = table.Column<string>(nullable: true),
                    PlateNumber = table.Column<string>(nullable: true),
                    TimeIn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayParking", x => x.ticketingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayParking");
        }
    }
}
