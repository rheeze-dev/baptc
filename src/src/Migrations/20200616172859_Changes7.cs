using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Changes7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "ShortTrip",
                newName: "RemarksOut");

            migrationBuilder.AddColumn<string>(
                name: "RemarksIn",
                table: "ShortTrip",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemarksIn",
                table: "ShortTrip");

            migrationBuilder.RenameColumn(
                name: "RemarksOut",
                table: "ShortTrip",
                newName: "Remarks");
        }
    }
}
