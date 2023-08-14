using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YZPortal.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsOpenFromPortalConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "PortalConfigs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOpen",
                table: "PortalConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
