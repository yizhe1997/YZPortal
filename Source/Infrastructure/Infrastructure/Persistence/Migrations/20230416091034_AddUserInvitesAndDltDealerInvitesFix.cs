using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserInvitesAndDltDealerInvitesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInviteDealerSelection_Dealers_DealerId",
                table: "UserInviteDealerSelection");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInviteDealerSelection_UserInvites_UserInviteId",
                table: "UserInviteDealerSelection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInviteDealerSelection",
                table: "UserInviteDealerSelection");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserInvites");

            migrationBuilder.RenameTable(
                name: "UserInviteDealerSelection",
                newName: "UserInviteDealerSelections");

            migrationBuilder.RenameIndex(
                name: "IX_UserInviteDealerSelection_UserInviteId",
                table: "UserInviteDealerSelections",
                newName: "IX_UserInviteDealerSelections_UserInviteId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInviteDealerSelection_DealerId",
                table: "UserInviteDealerSelections",
                newName: "IX_UserInviteDealerSelections_DealerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInviteDealerSelections",
                table: "UserInviteDealerSelections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInviteDealerSelections_Dealers_DealerId",
                table: "UserInviteDealerSelections",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInviteDealerSelections_UserInvites_UserInviteId",
                table: "UserInviteDealerSelections",
                column: "UserInviteId",
                principalTable: "UserInvites",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInviteDealerSelections_Dealers_DealerId",
                table: "UserInviteDealerSelections");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInviteDealerSelections_UserInvites_UserInviteId",
                table: "UserInviteDealerSelections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInviteDealerSelections",
                table: "UserInviteDealerSelections");

            migrationBuilder.RenameTable(
                name: "UserInviteDealerSelections",
                newName: "UserInviteDealerSelection");

            migrationBuilder.RenameIndex(
                name: "IX_UserInviteDealerSelections_UserInviteId",
                table: "UserInviteDealerSelection",
                newName: "IX_UserInviteDealerSelection_UserInviteId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInviteDealerSelections_DealerId",
                table: "UserInviteDealerSelection",
                newName: "IX_UserInviteDealerSelection_DealerId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserInvites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInviteDealerSelection",
                table: "UserInviteDealerSelection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInviteDealerSelection_Dealers_DealerId",
                table: "UserInviteDealerSelection",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInviteDealerSelection_UserInvites_UserInviteId",
                table: "UserInviteDealerSelection",
                column: "UserInviteId",
                principalTable: "UserInvites",
                principalColumn: "Id");
        }
    }
}
