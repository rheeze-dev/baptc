using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TicketingDB4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Ticketing");

            migrationBuilder.AddColumn<string>(
                name: "TypeOfPayment",
                table: "Ticketing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeOfPayment",
                table: "Ticketing");

            migrationBuilder.AddColumn<bool>(
                name: "Credit",
                table: "Ticketing",
                nullable: false,
                defaultValue: false);
        }
    }
}
