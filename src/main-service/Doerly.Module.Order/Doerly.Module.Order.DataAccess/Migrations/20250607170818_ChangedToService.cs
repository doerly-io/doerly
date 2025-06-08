using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "category_id",
                schema: "order",
                table: "order",
                newName: "service_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "service_id",
                schema: "order",
                table: "order",
                newName: "category_id");
        }
    }
}
