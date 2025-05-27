using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Profile_Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "profile");

            migrationBuilder.CreateTable(
                name: "language",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "profile",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    image_path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    cv_path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile", x => x.id);
                    table.UniqueConstraint("ak_profile_user_id", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "competence",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    category_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_competence", x => x.id);
                    table.ForeignKey(
                        name: "fk_competence_profile_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "language_proficiency",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_language_proficiency", x => x.id);
                    table.ForeignKey(
                        name: "fk_language_proficiency_language_language_id",
                        column: x => x.language_id,
                        principalSchema: "profile",
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_language_proficiency_profile_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                schema: "profile",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    reviewer_user_id = table.Column<int>(type: "integer", nullable: false),
                    profile_id = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_review", x => x.id);
                    table.CheckConstraint("ck_review_rating_range", "rating >= 1 AND rating <= 5");
                    table.ForeignKey(
                        name: "fk_review_profile_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_review_profile_reviewer_user_id",
                        column: x => x.reviewer_user_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_competence_profile_id_category_id",
                schema: "profile",
                table: "competence",
                columns: new[] { "profile_id", "category_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_language_code",
                schema: "profile",
                table: "language",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_language_proficiency_language_id",
                schema: "profile",
                table: "language_proficiency",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_language_proficiency_profile_id_language_id",
                schema: "profile",
                table: "language_proficiency",
                columns: new[] { "profile_id", "language_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_user_id",
                schema: "profile",
                table: "profile",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_review_id_reviewer_user_id",
                schema: "profile",
                table: "review",
                columns: new[] { "id", "reviewer_user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_review_profile_id_date_created_id",
                schema: "profile",
                table: "review",
                columns: new[] { "profile_id", "date_created", "id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_review_reviewer_user_id",
                schema: "profile",
                table: "review",
                column: "reviewer_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "competence",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "language_proficiency",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "review",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "language",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "profile",
                schema: "profile");
        }
    }
}
