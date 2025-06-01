using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Catalog.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitCatalogDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "category",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_category_category_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "catalog",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "filter",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_filter", x => x.id);
                    table.ForeignKey(
                        name: "fk_filter_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "catalog",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service", x => x.id);
                    table.ForeignKey(
                        name: "fk_service_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "catalog",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "service_filter_value",
                schema: "catalog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_id = table.Column<int>(type: "integer", nullable: false),
                    filter_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_filter_value", x => x.id);
                    table.ForeignKey(
                        name: "fk_service_filter_value_filter_filter_id",
                        column: x => x.filter_id,
                        principalSchema: "catalog",
                        principalTable: "filter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_filter_value_service_service_id",
                        column: x => x.service_id,
                        principalSchema: "catalog",
                        principalTable: "service",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_is_deleted",
                schema: "catalog",
                table: "category",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_category_is_enabled",
                schema: "catalog",
                table: "category",
                column: "is_enabled");

            migrationBuilder.CreateIndex(
                name: "ix_category_name",
                schema: "catalog",
                table: "category",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_category_parent_id",
                schema: "catalog",
                table: "category",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_filter_category_id",
                schema: "catalog",
                table: "filter",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_filter_name",
                schema: "catalog",
                table: "filter",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_service_category_id",
                schema: "catalog",
                table: "service",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_is_deleted",
                schema: "catalog",
                table: "service",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "ix_service_is_enabled",
                schema: "catalog",
                table: "service",
                column: "is_enabled");

            migrationBuilder.CreateIndex(
                name: "ix_service_name",
                schema: "catalog",
                table: "service",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_service_user_id",
                schema: "catalog",
                table: "service",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_filter_value_filter_id",
                schema: "catalog",
                table: "service_filter_value",
                column: "filter_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_filter_value_service_id",
                schema: "catalog",
                table: "service_filter_value",
                column: "service_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "service_filter_value",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "filter",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "service",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "category",
                schema: "catalog");
        }
    }
}
