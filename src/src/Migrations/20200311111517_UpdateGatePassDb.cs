using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class UpdateGatePassDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "GatePass",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "GatePass",
                newName: "StartDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "timeOut",
                table: "Ticketing",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "GatePass",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "GatePass",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "GatePass",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "GatePass",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdNumber",
                table: "GatePass",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IdType",
                table: "GatePass",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "GatePass",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GatePass",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "IdType",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GatePass");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "GatePass",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "GatePass",
                newName: "Name");

            migrationBuilder.AlterColumn<DateTime>(
                name: "timeOut",
                table: "Ticketing",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
