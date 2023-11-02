using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoContentAccessLevelpId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipContentAccessLevels_ContentAccessLevels_ContentAccessLevelpId",
                table: "MembershipContentAccessLevels");

            migrationBuilder.RenameColumn(
                name: "ContentAccessLevelpId",
                table: "MembershipContentAccessLevels",
                newName: "ContentAccessLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipContentAccessLevels_ContentAccessLevels_ContentAccessLevelId",
                table: "MembershipContentAccessLevels",
                column: "ContentAccessLevelId",
                principalTable: "ContentAccessLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipContentAccessLevels_ContentAccessLevels_ContentAccessLevelId",
                table: "MembershipContentAccessLevels");

            migrationBuilder.RenameColumn(
                name: "ContentAccessLevelId",
                table: "MembershipContentAccessLevels",
                newName: "ContentAccessLevelpId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipContentAccessLevels_ContentAccessLevels_ContentAccessLevelpId",
                table: "MembershipContentAccessLevels",
                column: "ContentAccessLevelpId",
                principalTable: "ContentAccessLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
