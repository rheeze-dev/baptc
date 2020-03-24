using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedTimeOutInTradingInspectorDb6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlateNumber1",
                table: "GatePass");

            migrationBuilder.RenameColumn(
                name: "PlateNumber2",
                table: "GatePass",
                newName: "PlateNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "GatePass",
                newName: "PlateNumber2");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumber1",
                table: "GatePass",
                nullable: true);
        }
    }
}
