using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class CurrentTicketDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentTicket",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    plateNumber = table.Column<string>(nullable: true),
                    typeOfTransaction = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentTicket", x => x.ticketingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentTicket");
        }
    }
}
