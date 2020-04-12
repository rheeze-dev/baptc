using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TotalDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "plateNumber",
                table: "Ticketing",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Total",
                columns: table => new
                {
                    ticketingId = table.Column<Guid>(nullable: false),
                    amount = table.Column<int>(nullable: false),
                    origin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Total", x => x.ticketingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Total");

            migrationBuilder.AlterColumn<string>(
                name: "plateNumber",
                table: "Ticketing",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
