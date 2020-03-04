using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class userRoleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Module",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "UserRole",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Modules",
                table: "UserRole",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modules",
                table: "UserRole");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "UserRole",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
