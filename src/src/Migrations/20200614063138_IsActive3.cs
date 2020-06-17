using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class IsActive3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Ticketing");

            migrationBuilder.AddColumn<string>(
                name: "Transaction",
                table: "Ticketing",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transaction",
                table: "Ticketing");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Ticketing",
                nullable: false,
                defaultValue: false);
        }
    }
}
