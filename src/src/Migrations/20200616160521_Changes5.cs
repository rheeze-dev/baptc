using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Changes5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inspector",
                table: "ShortTrip",
                newName: "InspectorOut");

            migrationBuilder.RenameColumn(
                name: "EstimatedVolume",
                table: "ShortTrip",
                newName: "EstimatedVolumeOut");

            migrationBuilder.RenameColumn(
                name: "Commodity",
                table: "ShortTrip",
                newName: "CommodityOut");

            migrationBuilder.AddColumn<string>(
                name: "CommodityIn",
                table: "ShortTrip",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedVolumeIn",
                table: "ShortTrip",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InspectorIn",
                table: "ShortTrip",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommodityIn",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "EstimatedVolumeIn",
                table: "ShortTrip");

            migrationBuilder.DropColumn(
                name: "InspectorIn",
                table: "ShortTrip");

            migrationBuilder.RenameColumn(
                name: "InspectorOut",
                table: "ShortTrip",
                newName: "Inspector");

            migrationBuilder.RenameColumn(
                name: "EstimatedVolumeOut",
                table: "ShortTrip",
                newName: "EstimatedVolume");

            migrationBuilder.RenameColumn(
                name: "CommodityOut",
                table: "ShortTrip",
                newName: "Commodity");
        }
    }
}
