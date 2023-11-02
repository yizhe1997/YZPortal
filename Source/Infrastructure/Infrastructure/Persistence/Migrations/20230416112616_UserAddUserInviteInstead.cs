using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserAddUserInviteInstead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvites_AspNetUsers_UserId",
                table: "UserInvites");

            migrationBuilder.DropIndex(
                name: "IX_UserInvites_UserId",
                table: "UserInvites");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserInvites");

            migrationBuilder.DropColumn(
                name: "MembershipId",
                table: "UserInviteDealerSelections");

            migrationBuilder.AddColumn<Guid>(
                name: "UserInviteId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserInviteId",
                table: "AspNetUsers",
                column: "UserInviteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserInvites_UserInviteId",
                table: "AspNetUsers",
                column: "UserInviteId",
                principalTable: "UserInvites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserInvites_UserInviteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserInviteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserInviteId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserInvites",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MembershipId",
                table: "UserInviteDealerSelections",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInvites_UserId",
                table: "UserInvites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvites_AspNetUsers_UserId",
                table: "UserInvites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
