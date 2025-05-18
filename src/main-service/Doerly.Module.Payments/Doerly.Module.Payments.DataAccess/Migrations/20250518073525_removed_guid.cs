using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Payments.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removed_guid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bill_guid",
                schema: "payment",
                table: "bill");

            migrationBuilder.DropColumn(
                name: "guid",
                schema: "payment",
                table: "bill");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                schema: "payment",
                table: "bill",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_bill_guid",
                schema: "payment",
                table: "bill",
                column: "guid",
                unique: true);
        }
    }
}
