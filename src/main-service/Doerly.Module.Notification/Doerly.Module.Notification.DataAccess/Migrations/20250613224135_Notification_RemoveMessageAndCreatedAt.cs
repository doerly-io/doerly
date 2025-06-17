using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Notification.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Notification_RemoveMessageAndCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_notification_created_at",
                schema: "notification",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "created_at",
                schema: "notification",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "title",
                schema: "notification",
                table: "notification");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                schema: "notification",
                table: "notification",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.CreateIndex(
                name: "ix_notification_date_created",
                schema: "notification",
                table: "notification",
                column: "date_created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_notification_date_created",
                schema: "notification",
                table: "notification");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                schema: "notification",
                table: "notification",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                schema: "notification",
                table: "notification",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "title",
                schema: "notification",
                table: "notification",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_notification_created_at",
                schema: "notification",
                table: "notification",
                column: "created_at");
        }
    }
}
