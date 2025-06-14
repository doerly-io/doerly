using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Notification.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Notification_AddTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "timestamp",
                schema: "notification",
                table: "notification",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timestamp",
                schema: "notification",
                table: "notification");
        }
    }
}
