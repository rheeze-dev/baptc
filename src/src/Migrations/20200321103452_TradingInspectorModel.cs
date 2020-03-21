using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TradingInspectorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FarmersTruck",
                table: "FarmersTruck");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FarmersTruck");

            migrationBuilder.AlterColumn<int>(
                name: "EstimatedVolume",
                table: "ShortTrip",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "ShortTrip",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Volume",
                table: "FarmersTruck",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "FarmersTruck",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<Guid>(
                name: "ticketingId",
                table: "FarmersTruck",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeIn",
                table: "FarmersTruck",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip",
                column: "ticketingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FarmersTruck",
                table: "FarmersTruck",
                column: "ticketingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FarmersTruck",
                table: "FarmersTruck");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "ticketingId",
                table: "FarmersTruck");

            migrationBuilder.DropColumn(
                name: "TimeIn",
                table: "FarmersTruck");

            migrationBuilder.AlterColumn<int>(
                name: "EstimatedVolume",
                table: "ShortTrip",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShortTrip",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Volume",
                table: "FarmersTruck",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "FarmersTruck",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FarmersTruck",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortTrip",
                table: "ShortTrip",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FarmersTruck",
                table: "FarmersTruck",
                column: "Id");
        }
    }
}
