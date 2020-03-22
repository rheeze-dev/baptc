using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class UpdateDbGatePass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GatePass",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "IdType",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "GatePass");

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "GatePass",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GatePass",
                table: "GatePass",
                column: "ticketingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GatePass",
                table: "GatePass");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "GatePass");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GatePass",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "GatePass",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
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

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "GatePass",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GatePass",
                table: "GatePass",
                column: "Id");
        }
    }
}
