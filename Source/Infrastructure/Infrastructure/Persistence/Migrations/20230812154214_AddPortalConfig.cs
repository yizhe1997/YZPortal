using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPortalConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubjectIdentifier",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_SubjectIdentifier",
                table: "AspNetUsers",
                column: "SubjectIdentifier");

            migrationBuilder.CreateTable(
                name: "PortalConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newid()"),
                    UseTabSet = table.Column<bool>(type: "bit", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    IsFixedHeader = table.Column<bool>(type: "bit", nullable: false),
                    IsFixedFooter = table.Column<bool>(type: "bit", nullable: false),
                    IsFullSide = table.Column<bool>(type: "bit", nullable: false),
                    ShowFooter = table.Column<bool>(type: "bit", nullable: false),
                    UserSubjectIdentifier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "null"),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "unknown")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PortalConfigs_AspNetUsers_UserSubjectIdentifier",
                        column: x => x.UserSubjectIdentifier,
                        principalTable: "AspNetUsers",
                        principalColumn: "SubjectIdentifier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PortalConfigs_UserSubjectIdentifier",
                table: "PortalConfigs",
                column: "UserSubjectIdentifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortalConfigs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_SubjectIdentifier",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectIdentifier",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
