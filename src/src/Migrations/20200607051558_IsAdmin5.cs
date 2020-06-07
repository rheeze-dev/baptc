using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class IsAdmin5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Total",
                table: "Total");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Total",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Total",
                table: "Total",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Total",
                table: "Total");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Total");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Total",
                table: "Total",
                column: "ticketingId");
        }
    }
}
