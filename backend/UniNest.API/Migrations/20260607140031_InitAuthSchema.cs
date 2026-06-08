using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniNest.API.Migrations
{
    /// <inheritdoc />
    public partial class InitAuthSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ListingImages_RoomListingId",
                table: "ListingImages");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RoomListings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RoomListings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomListings_CreatedAt",
                table: "RoomListings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_RoomListings_MonthlyRent",
                table: "RoomListings",
                column: "MonthlyRent");

            migrationBuilder.CreateIndex(
                name: "IX_RoomListings_PreferredGender",
                table: "RoomListings",
                column: "PreferredGender");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_RoomListingId_IsPrimary",
                table: "ListingImages",
                columns: new[] { "RoomListingId", "IsPrimary" },
                unique: true,
                filter: "\"IsPrimary\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomListings_CreatedAt",
                table: "RoomListings");

            migrationBuilder.DropIndex(
                name: "IX_RoomListings_MonthlyRent",
                table: "RoomListings");

            migrationBuilder.DropIndex(
                name: "IX_RoomListings_PreferredGender",
                table: "RoomListings");

            migrationBuilder.DropIndex(
                name: "IX_ListingImages_RoomListingId_IsPrimary",
                table: "ListingImages");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RoomListings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "RoomListings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.CreateIndex(
                name: "IX_ListingImages_RoomListingId",
                table: "ListingImages",
                column: "RoomListingId");
        }
    }
}
