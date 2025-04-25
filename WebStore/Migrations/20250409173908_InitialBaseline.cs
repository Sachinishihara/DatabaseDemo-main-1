using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebStore.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:public.discount_type", "percentage,flat");

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "discount_codes",
                columns: table => new
                {
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DiscountType = table.Column<int>(type: "discount_type", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MaxUsage = table.Column<int>(type: "integer", nullable: true),
                    TimesUsed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discount_codes", x => x.DiscountCodeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_DiscountCodeId",
                table: "orders",
                column: "DiscountCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_discount_codes_DiscountCodeId",
                table: "orders",
                column: "DiscountCodeId",
                principalTable: "discount_codes",
                principalColumn: "DiscountCodeId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_discount_codes_DiscountCodeId",
                table: "orders");

            migrationBuilder.DropTable(
                name: "discount_codes");

            migrationBuilder.DropIndex(
                name: "IX_orders_DiscountCodeId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "orders");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:public.discount_type", "percentage,flat");
        }
    }
}
