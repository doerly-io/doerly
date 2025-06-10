using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Communication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConversationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "conversation_name",
                schema: "communication",
                table: "conversation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "conversation_name",
                schema: "communication",
                table: "conversation",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
