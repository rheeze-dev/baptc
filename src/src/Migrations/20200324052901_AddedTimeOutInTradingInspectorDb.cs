using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedTimeOutInTradingInspectorDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "TradersTruck",
                newName: "TimeOut");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ShortTrip",
                newName: "DateInspected");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "PayParking",
                newName: "TimeOut");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "FarmersTruck",
                newName: "TimeOut");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInspected",
                table: "TradersTruck",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInspected",
                table: "PayParking",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateInspected",
                table: "FarmersTruck",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateInspected",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "DateInspected",
                table: "PayParking");

            migrationBuilder.DropColumn(
                name: "DateInspected",
                table: "FarmersTruck");

            migrationBuilder.RenameColumn(
                name: "TimeOut",
                table: "TradersTruck",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "DateInspected",
                table: "ShortTrip",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "TimeOut",
                table: "PayParking",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "TimeOut",
                table: "FarmersTruck",
                newName: "Date");
        }
    }
}
