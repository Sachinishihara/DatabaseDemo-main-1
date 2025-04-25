using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_carrier",
                table: "orders");

            migrationBuilder.DropTable(
                name: "carriers");

            migrationBuilder.DropIndex(
                name: "IX_orders_CarrierId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "CarrierId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "delivered_date",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "shipped_date",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "tracking_number",
                table: "orders");
        }
    }
}
