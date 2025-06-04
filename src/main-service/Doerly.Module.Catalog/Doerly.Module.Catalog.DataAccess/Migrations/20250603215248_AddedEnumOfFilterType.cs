using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Catalog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnumOfFilterType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_service_filter_value_filter_id",
                schema: "catalog",
                table: "service_filter_value");

            migrationBuilder.AlterColumn<byte>(
                name: "type",
                schema: "catalog",
                table: "filter",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "ix_service_filter_value_filter_id_value",
                schema: "catalog",
                table: "service_filter_value",
                columns: new[] { "filter_id", "value" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_service_filter_value_filter_id_value",
                schema: "catalog",
                table: "service_filter_value");

            migrationBuilder.AlterColumn<int>(
                name: "type",
                schema: "catalog",
                table: "filter",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.CreateIndex(
                name: "ix_service_filter_value_filter_id",
                schema: "catalog",
                table: "service_filter_value",
                column: "filter_id");
        }
    }
}
