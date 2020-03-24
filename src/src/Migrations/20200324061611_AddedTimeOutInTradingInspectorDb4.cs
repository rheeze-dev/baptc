using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedTimeOutInTradingInspectorDb4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StallLease",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    DriverName = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    PlateNumber1 = table.Column<string>(nullable: true),
                    PlateNumber2 = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StallLease", x => x.ticketingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StallLease");
        }
    }
}
