using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class UpdateTradersTruckModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck");

            migrationBuilder.DropIndex(
                name: "IX_TradersTruck_ticketingId",
                table: "TradersTruck");

            migrationBuilder.AlterColumn<string>(
                name: "ticketingId",
                table: "TradersTruck",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId1",
                table: "TradersTruck",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradersTruck_ticketingId1",
                table: "TradersTruck",
                column: "ticketingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId1",
                table: "TradersTruck",
                column: "ticketingId1",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId1",
                table: "TradersTruck");

            migrationBuilder.DropIndex(
                name: "IX_TradersTruck_ticketingId1",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "ticketingId1",
                table: "TradersTruck");

            migrationBuilder.AlterColumn<Guid>(
                name: "ticketingId",
                table: "TradersTruck",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TradersTruck_ticketingId",
                table: "TradersTruck",
                column: "ticketingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck",
                column: "ticketingId",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
