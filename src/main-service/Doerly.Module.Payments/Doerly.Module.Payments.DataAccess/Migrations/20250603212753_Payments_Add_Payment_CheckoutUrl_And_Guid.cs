using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Payments.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Payments_Add_Payment_CheckoutUrl_And_Guid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "action",
                schema: "payment",
                table: "payment",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "checkout_url",
                schema: "payment",
                table: "payment",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "payment",
                table: "payment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_payment_guid",
                schema: "payment",
                table: "payment",
                column: "guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_payment_guid_status",
                schema: "payment",
                table: "payment",
                columns: new[] { "guid", "status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_payment_guid",
                schema: "payment",
                table: "payment");

            migrationBuilder.DropIndex(
                name: "ix_payment_guid_status",
                schema: "payment",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "checkout_url",
                schema: "payment",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "payment",
                table: "payment");

            migrationBuilder.AlterColumn<int>(
                name: "action",
                schema: "payment",
                table: "payment",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
