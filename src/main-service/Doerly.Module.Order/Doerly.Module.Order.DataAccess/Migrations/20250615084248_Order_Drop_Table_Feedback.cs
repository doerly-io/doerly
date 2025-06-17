using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Order_Drop_Table_Feedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_feedback",
                schema: "order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "order_feedback",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    reviewer_user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_feedback", x => x.id);
                    table.CheckConstraint("ck_feedback_rating_range", "\"rating\" >= 1 AND \"rating\" <= 5");
                    table.ForeignKey(
                        name: "fk_order_feedback_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_feedback_last_modified_date",
                schema: "order",
                table: "order_feedback",
                column: "last_modified_date");

            migrationBuilder.CreateIndex(
                name: "ix_order_feedback_order_id",
                schema: "order",
                table: "order_feedback",
                column: "order_id",
                unique: true);
        }
    }
}
