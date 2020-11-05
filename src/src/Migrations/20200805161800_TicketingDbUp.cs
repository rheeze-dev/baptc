using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TicketingDbUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PullOut",
                table: "Ticketing",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeOfEntry",
                table: "Ticketing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PullOut",
                table: "Ticketing");

            migrationBuilder.DropColumn(
                name: "TypeOfEntry",
                table: "Ticketing");
        }
    }
}
