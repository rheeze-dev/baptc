using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Modulesselected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "Modules",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "Modules");
        }
    }
}
