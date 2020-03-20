using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class FarmerTruckModelForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "FarmersTruck",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmersTruck_Ticketing_ticketingId",
                table: "FarmersTruck");

            migrationBuilder.DropIndex(
                name: "IX_FarmersTruck_ticketingId",
                table: "FarmersTruck");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "FarmersTruck");
        }
    }
}
