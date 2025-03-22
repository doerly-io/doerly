using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Communication.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Communication_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "communication");

            migrationBuilder.EnsureSchema(
                name: "profile");

            migrationBuilder.CreateTable(
                name: "profile",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    image_path = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "conversation",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conversation_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    initiator_id = table.Column<int>(type: "integer", nullable: false),
                    recipient_id = table.Column<int>(type: "integer", nullable: false),
                    last_message_id = table.Column<int>(type: "integer", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_conversation", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversation_profile_initiator_id",
                        column: x => x.initiator_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_conversation_profile_recipient_id",
                        column: x => x.recipient_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "message",
                schema: "communication",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conversation_id = table.Column<int>(type: "integer", nullable: false),
                    message_type = table.Column<int>(type: "integer", nullable: false),
                    sender_id = table.Column<int>(type: "integer", nullable: false),
                    message_content = table.Column<string>(type: "text", nullable: false),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_message", x => x.id);
                    table.ForeignKey(
                        name: "fk_message_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "communication",
                        principalTable: "conversation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_message_profile_sender_id",
                        column: x => x.sender_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_conversation_initiator_id",
                schema: "communication",
                table: "conversation",
                column: "initiator_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversation_last_message_id",
                schema: "communication",
                table: "conversation",
                column: "last_message_id");

            migrationBuilder.CreateIndex(
                name: "ix_conversation_recipient_id",
                schema: "communication",
                table: "conversation",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "ix_message_conversation_id",
                schema: "communication",
                table: "message",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_message_sender_id",
                schema: "communication",
                table: "message",
                column: "sender_id");

            migrationBuilder.AddForeignKey(
                name: "fk_conversation_messages_last_message_id",
                schema: "communication",
                table: "conversation",
                column: "last_message_id",
                principalSchema: "communication",
                principalTable: "message",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_conversation_messages_last_message_id",
                schema: "communication",
                table: "conversation");

            migrationBuilder.DropTable(
                name: "message",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "conversation",
                schema: "communication");

            migrationBuilder.DropTable(
                name: "profile",
                schema: "profile");
        }
    }
}
