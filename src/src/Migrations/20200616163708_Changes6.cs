using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Changes6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateInspected",
                table: "ShortTrip",
                newName: "DateInspectedOut");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInspectedIn",
                table: "ShortTrip",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateInspectedIn",
                table: "ShortTrip");

            migrationBuilder.RenameColumn(
                name: "DateInspectedOut",
                table: "ShortTrip",
                newName: "DateInspected");
        }
    }
}
