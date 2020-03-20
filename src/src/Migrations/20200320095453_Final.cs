using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck");

            migrationBuilder.AlterColumn<Guid>(
                name: "ticketingId",
                table: "TradersTruck",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck",
                column: "ticketingId",
                principalTable: "Ticketing",
                principalColumn: "ticketingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradersTruck_Ticketing_ticketingId",
                table: "TradersTruck");

            migrationBuilder.AlterColumn<Guid>(
                name: "ticketingId",
                table: "TradersTruck",
                nullable: true,
                oldClrType: typeof(Guid));

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
