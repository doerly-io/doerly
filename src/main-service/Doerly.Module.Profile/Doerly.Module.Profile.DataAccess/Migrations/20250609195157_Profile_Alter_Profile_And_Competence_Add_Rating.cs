using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doerly.Module.Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Profile_Alter_Profile_And_Competence_Add_Rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "rating",
                schema: "profile",
                table: "profile",
                type: "real",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "rating",
                schema: "profile",
                table: "competence",
                type: "real",
                precision: 3,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_profile_rating",
                schema: "profile",
                table: "profile",
                column: "rating");

            migrationBuilder.AddCheckConstraint(
                name: "ck_profile_rating_range",
                schema: "profile",
                table: "profile",
                sql: "\"rating\" >= 1 AND \"rating\" <= 5");

            migrationBuilder.CreateIndex(
                name: "ix_competence_rating",
                schema: "profile",
                table: "competence",
                column: "rating");

            migrationBuilder.AddCheckConstraint(
                name: "ck_competence_rating_range",
                schema: "profile",
                table: "competence",
                sql: "\"rating\" >= 1 AND \"rating\" <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_profile_rating",
                schema: "profile",
                table: "profile");

            migrationBuilder.DropCheckConstraint(
                name: "ck_profile_rating_range",
                schema: "profile",
                table: "profile");

            migrationBuilder.DropIndex(
                name: "ix_competence_rating",
                schema: "profile",
                table: "competence");

            migrationBuilder.DropCheckConstraint(
                name: "ck_competence_rating_range",
                schema: "profile",
                table: "competence");

            migrationBuilder.DropColumn(
                name: "rating",
                schema: "profile",
                table: "profile");

            migrationBuilder.DropColumn(
                name: "rating",
                schema: "profile",
                table: "competence");
        }
    }
}
