using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Payments.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class maxlength1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "payer_id",
                schema: "payment",
                table: "bill",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "payer_id",
                schema: "payment",
                table: "bill");
        }
    }
}
