using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Profile_Create_Table_Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "ak_profile_user_id",
                schema: "profile",
                table: "profile",
                column: "user_id");

            migrationBuilder.CreateTable(
                name: "feedback",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    reviewer_user_id = table.Column<int>(type: "integer", nullable: false),
                    reviewee_user_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedback", x => x.id);
                    table.CheckConstraint("ck_feedback_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5");
                    table.ForeignKey(
                        name: "fk_feedback_profile_reviewee_user_id",
                        column: x => x.reviewee_user_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_feedback_profile_reviewer_user_id",
                        column: x => x.reviewer_user_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_feedback_last_modified_date",
                schema: "profile",
                table: "feedback",
                column: "last_modified_date");

            migrationBuilder.CreateIndex(
                name: "ix_feedback_reviewee_user_id",
                schema: "profile",
                table: "feedback",
                column: "reviewee_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_feedback_reviewer_user_id",
                schema: "profile",
                table: "feedback",
                column: "reviewer_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback",
                schema: "profile");

            migrationBuilder.DropUniqueConstraint(
                name: "ak_profile_user_id",
                schema: "profile",
                table: "profile");
        }
    }
}
