using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Notification.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Notification_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "notification",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    type = table.Column<byte>(type: "smallint", maxLength: 50, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_notification_created_at",
                schema: "notification",
                table: "notification",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_notification_user_id",
                schema: "notification",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_user_id_is_read",
                schema: "notification",
                table: "notification",
                columns: new[] { "user_id", "is_read" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification",
                schema: "notification");
        }
    }
}
