using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmersTruck_Ticketing_ticketingId",
                table: "FarmersTruck");

            migrationBuilder.DropForeignKey(
                name: "FK_ShortTrip_Ticketing_ticketingId",
                table: "ShortTrip");

            migrationBuilder.DropIndex(
                name: "IX_ShortTrip_ticketingId",
                table: "ShortTrip");

            migrationBuilder.DropIndex(
                name: "IX_FarmersTruck_ticketingId",
                table: "FarmersTruck");

            migrationBuilder.DropColumn(
                name: "Ticketing",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "FarmersTruck");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Ticketing",
                table: "TradersTruck",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "FarmersTruck",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortTrip_ticketingId",
                table: "ShortTrip",
                column: "ticketingId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmersTruck_ticketingId",
                table: "FarmersTruck",
                column: "ticketingId");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmersTruck_Ticketing_ticketingId",
                table: "FarmersTruck",
                column: "ticketingId",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShortTrip_Ticketing_ticketingId",
                table: "ShortTrip",
                column: "ticketingId",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
