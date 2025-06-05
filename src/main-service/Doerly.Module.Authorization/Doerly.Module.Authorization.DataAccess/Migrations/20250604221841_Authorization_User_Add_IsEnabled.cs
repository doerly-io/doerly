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
            migrationBuilder.DropIndex(
                name: "ix_user_email",
                schema: "auth",
                table: "user");

            migrationBuilder.AddColumn<bool>(
                name: "is_enabled",
                schema: "auth",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_user_email_is_email_verified",
                schema: "auth",
                table: "user",
                columns: new[] { "email", "is_email_verified" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_is_enabled",
                schema: "auth",
                table: "user",
                column: "is_enabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_user_email_is_email_verified",
                schema: "auth",
                table: "user");

            migrationBuilder.DropIndex(
                name: "ix_user_is_enabled",
                schema: "auth",
                table: "user");

            migrationBuilder.DropColumn(
                name: "is_enabled",
                schema: "auth",
                table: "user");

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                schema: "auth",
                table: "user",
                column: "email",
                unique: true);
        }
    }
}
