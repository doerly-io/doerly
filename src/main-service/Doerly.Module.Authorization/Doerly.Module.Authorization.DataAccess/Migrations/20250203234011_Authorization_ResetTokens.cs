using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Authorization.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Authorization_ResetTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reset_token",
                schema: "authorization",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reset_token", x => x.guid);
                    table.ForeignKey(
                        name: "fk_reset_token_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "authorization",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reset_token_user_id",
                schema: "authorization",
                table: "reset_token",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reset_token",
                schema: "authorization");
        }
    }
}
