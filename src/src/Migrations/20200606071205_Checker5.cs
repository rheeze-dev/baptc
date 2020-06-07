using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Checker5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassVariety",
                table: "Commodities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Commodities",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Sitio",
                table: "AccreditedIndividualFarmers",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassVariety",
                table: "Commodities");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Commodities");

            migrationBuilder.AlterColumn<string>(
                name: "Sitio",
                table: "AccreditedIndividualFarmers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
