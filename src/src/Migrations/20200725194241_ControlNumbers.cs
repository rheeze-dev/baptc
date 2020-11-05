using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class ControlNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "TradersTruck",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "StallLease",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "ShortTrip",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "SecurityInspectionReport",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "PriceCommodity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "average",
                table: "PriceCommodity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "GatePass",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControlNumber",
                table: "FarmersTruck",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "StallLease");

            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "SecurityInspectionReport");

            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "PriceCommodity");

            migrationBuilder.DropColumn(
                name: "average",
                table: "PriceCommodity");

            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "ControlNumber",
                table: "FarmersTruck");
        }
    }
}
