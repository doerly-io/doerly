using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "address");

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
                name: "region",
                schema: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    country = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_region", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "city",
                schema: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    region_id = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_city", x => x.id);
                    table.ForeignKey(
                        name: "fk_city_region_region_id",
                        column: x => x.region_id,
                        principalSchema: "address",
                        principalTable: "region",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "street",
                schema: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    zip_code = table.Column<string>(type: "text", nullable: false),
                    region_id = table.Column<int>(type: "integer", nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_street", x => x.id);
                    table.ForeignKey(
                        name: "fk_street_city_city_id",
                        column: x => x.city_id,
                        principalSchema: "address",
                        principalTable: "city",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_street_region_region_id",
                        column: x => x.region_id,
                        principalSchema: "address",
                        principalTable: "region",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    street_id = table.Column<int>(type: "integer", nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    sex = table.Column<int>(type: "integer", nullable: false),
                    bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    image_path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    cv_path = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profile", x => x.id);
                    table.ForeignKey(
                        name: "fk_profile_street_street_id",
                        column: x => x.street_id,
                        principalSchema: "address",
                        principalTable: "street",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "language_proficiencies",
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
                    table.PrimaryKey("pk_language_proficiencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_language_proficiencies_language_language_id",
                        column: x => x.language_id,
                        principalSchema: "profile",
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_language_proficiencies_profile_profile_id",
                        column: x => x.profile_id,
                        principalSchema: "profile",
                        principalTable: "profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_city_region_id",
                schema: "address",
                table: "city",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "ix_language_code",
                schema: "profile",
                table: "language",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_language_proficiencies_language_id",
                schema: "profile",
                table: "language_proficiencies",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_language_proficiencies_profile_id_language_id",
                schema: "profile",
                table: "language_proficiencies",
                columns: new[] { "profile_id", "language_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_street_id",
                schema: "profile",
                table: "profile",
                column: "street_id");

            migrationBuilder.CreateIndex(
                name: "ix_profile_user_id",
                schema: "profile",
                table: "profile",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_street_city_id",
                schema: "address",
                table: "street",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_street_region_id",
                schema: "address",
                table: "street",
                column: "region_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "language_proficiencies",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "language",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "profile",
                schema: "profile");

            migrationBuilder.DropTable(
                name: "street",
                schema: "address");

            migrationBuilder.DropTable(
                name: "city",
                schema: "address");

            migrationBuilder.DropTable(
                name: "region",
                schema: "address");
        }
    }
}
