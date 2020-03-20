using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class ChangeDbTrading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TradersTruck",
                table: "TradersTruck");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TradersTruck");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradersTruck",
                table: "TradersTruck",
                column: "ticketingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TradersTruck",
                table: "TradersTruck");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TradersTruck",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradersTruck",
                table: "TradersTruck",
                column: "Id");
        }
    }
}
