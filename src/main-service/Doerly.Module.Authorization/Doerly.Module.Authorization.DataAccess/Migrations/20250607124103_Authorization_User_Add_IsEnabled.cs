using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Authorization.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Authorization_User_Add_IsEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "is_enabled",
                schema: "auth",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "is_enabled",
                schema: "auth",
                table: "user",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }
    }
}
