using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Payments.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Payments_Add_CardNumber_And_PaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "card_number",
                schema: "payment",
                table: "payment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "payment_method",
                schema: "payment",
                table: "payment",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "card_number",
                schema: "payment",
                table: "payment");

            migrationBuilder.DropColumn(
                name: "payment_method",
                schema: "payment",
                table: "payment");
        }
    }
}
