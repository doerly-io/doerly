using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSizeFieldToOrderFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "file_size",
                schema: "order",
                table: "order_file",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_size",
                schema: "order",
                table: "order_file");
        }
    }
}
