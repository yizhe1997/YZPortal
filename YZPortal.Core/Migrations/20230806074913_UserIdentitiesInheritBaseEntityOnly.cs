using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YZPortal.Core.Migrations
{
    /// <inheritdoc />
    public partial class UserIdentitiesInheritBaseEntityOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Identity_AspNetUsers_UserId",
                table: "Identity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Identity",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Identity");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Identity");

            migrationBuilder.RenameTable(
                name: "Identity",
                newName: "Identities");

            migrationBuilder.RenameIndex(
                name: "IX_Identity_UserId",
                table: "Identities",
                newName: "IX_Identities_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identities",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "newid()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Identities",
                table: "Identities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Identities_AspNetUsers_UserId",
                table: "Identities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Identities_AspNetUsers_UserId",
                table: "Identities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Identities",
                table: "Identities");

            migrationBuilder.RenameTable(
                name: "Identities",
                newName: "Identity");

            migrationBuilder.RenameIndex(
                name: "IX_Identities_UserId",
                table: "Identity",
                newName: "IX_Identity_UserId");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Identity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Identity",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "unknown");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Identity",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Identity",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "unknown");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Identity",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "null");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Identity",
                table: "Identity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_AspNetUsers_UserId",
                table: "Identity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
