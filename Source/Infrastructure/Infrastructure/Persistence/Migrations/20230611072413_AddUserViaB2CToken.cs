using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserViaB2CToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserInvites_UserInviteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserInviteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Admin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserInviteId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TokenSubClaim",
                table: "AspNetUsers",
                newName: "SubjectIdentifier");

            migrationBuilder.RenameColumn(
                name: "LastLoggedIn",
                table: "AspNetUsers",
                newName: "AuthTime");

            migrationBuilder.AlterColumn<string>(
                name: "IdentityProvider",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AuthClassRef",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthExpireTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastidpAccessToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthClassRef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AuthExpireTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastidpAccessToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SubjectIdentifier",
                table: "AspNetUsers",
                newName: "TokenSubClaim");

            migrationBuilder.RenameColumn(
                name: "AuthTime",
                table: "AspNetUsers",
                newName: "LastLoggedIn");

            migrationBuilder.AlterColumn<int>(
                name: "IdentityProvider",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Admin",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
