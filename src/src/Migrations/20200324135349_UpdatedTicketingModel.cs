using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class UpdatedTicketingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "controlNumber",
                table: "Ticketing",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "issuingClerk",
                table: "Ticketing",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "receivingClerk",
                table: "Ticketing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "controlNumber",
                table: "Ticketing");

            migrationBuilder.DropColumn(
                name: "issuingClerk",
                table: "Ticketing");

            migrationBuilder.DropColumn(
                name: "receivingClerk",
                table: "Ticketing");
        }
    }
}
