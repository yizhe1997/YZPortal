using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserProfileImageConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserProfileImages_UserProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserProfileImageId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "RefId",
                table: "UserProfileImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileImages_RefId",
                table: "UserProfileImages",
                column: "RefId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfileImages_AspNetUsers_RefId",
                table: "UserProfileImages",
                column: "RefId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfileImages_AspNetUsers_RefId",
                table: "UserProfileImages");

            migrationBuilder.DropIndex(
                name: "IX_UserProfileImages_RefId",
                table: "UserProfileImages");

            migrationBuilder.DropColumn(
                name: "RefId",
                table: "UserProfileImages");

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileImageId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserProfileImageId",
                table: "AspNetUsers",
                column: "UserProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserProfileImages_UserProfileImageId",
                table: "AspNetUsers",
                column: "UserProfileImageId",
                principalTable: "UserProfileImages",
                principalColumn: "Id");
        }
    }
}
