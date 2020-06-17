using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Changes4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShortTrip",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShortTrip");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip",
                column: "ticketingId");
        }
    }
}
