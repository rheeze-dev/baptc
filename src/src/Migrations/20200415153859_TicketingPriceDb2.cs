using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TicketingPriceDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmersTruckPrice",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "PayParkingPrice",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "ShortTripPrice",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "TradersTruckPrice",
                table: "TicketingPrice");

            migrationBuilder.AddColumn<int>(
                name: "FarmersTruckDoubleTire",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FarmersTruckSingleTire",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayParkingDaytime",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayParkingOvernight",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShortTripDelivery",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShortTripPickUp",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TradersTruckWithTransaction",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TradersTruckWithoutTransaction",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmersTruckDoubleTire",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "FarmersTruckSingleTire",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "PayParkingDaytime",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "PayParkingOvernight",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "ShortTripDelivery",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "ShortTripPickUp",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "TradersTruckWithTransaction",
                table: "TicketingPrice");

            migrationBuilder.DropColumn(
                name: "TradersTruckWithoutTransaction",
                table: "TicketingPrice");

            migrationBuilder.AddColumn<int>(
                name: "FarmersTruckPrice",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PayParkingPrice",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShortTripPrice",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TradersTruckPrice",
                table: "TicketingPrice",
                nullable: false,
                defaultValue: 0);
        }
    }
}
