using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "TradersTruck",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradersTruck_ticketingId",
                table: "TradersTruck",
                column: "ticketingId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortTrip_ticketingId",
                table: "ShortTrip",
                column: "ticketingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShortTrip_Ticketing_ticketingId",
                table: "ShortTrip",
                column: "ticketingId",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck",
                column: "ticketingId",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShortTrip_Ticketing_ticketingId",
                table: "ShortTrip");

            migrationBuilder.DropForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck");

            migrationBuilder.DropIndex(
                name: "IX_TradersTruck_ticketingId",
                table: "TradersTruck");

            migrationBuilder.DropIndex(
                name: "IX_ShortTrip_ticketingId",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "ShortTrip");
        }
    }
}
