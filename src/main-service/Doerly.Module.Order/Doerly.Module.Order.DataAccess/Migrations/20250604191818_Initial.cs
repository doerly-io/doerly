﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Doerly.Module.Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.CreateTable(
                name: "order",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    code = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    price = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    is_price_negotiable = table.Column<bool>(type: "boolean", nullable: false),
                    payment_kind = table.Column<byte>(type: "smallint", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    customer_completion_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    executor_id = table.Column<int>(type: "integer", nullable: true),
                    executor_completion_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    execution_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    bill_id = table.Column<int>(type: "integer", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "execution_proposal",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    sender_id = table.Column<int>(type: "integer", nullable: false),
                    receiver_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_execution_proposal", x => x.id);
                    table.ForeignKey(
                        name: "fk_execution_proposal_orders_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_file",
                schema: "order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    path = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_file", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_file_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "order",
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_execution_proposal_order_id",
                schema: "order",
                table: "execution_proposal",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_execution_proposal_receiver_id",
                schema: "order",
                table: "execution_proposal",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "ix_execution_proposal_sender_id",
                schema: "order",
                table: "execution_proposal",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_file_order_id",
                schema: "order",
                table: "order_file",
                column: "order_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "execution_proposal",
                schema: "order");

            migrationBuilder.DropTable(
                name: "order_file",
                schema: "order");

            migrationBuilder.DropTable(
                name: "order",
                schema: "order");
        }
    }
}
