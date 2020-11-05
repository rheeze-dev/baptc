using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class editedData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "StallLease",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StallNumber",
                table: "StallLease",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "StallLease");

            migrationBuilder.DropColumn(
                name: "StallNumber",
                table: "StallLease");
        }
    }
}
