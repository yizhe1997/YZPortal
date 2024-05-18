using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProductAndProductCategoryAddField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryPicture_ProductCategories_RefId",
                table: "ProductCategoryPicture");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategoryPicture",
                table: "ProductCategoryPicture");

            migrationBuilder.RenameTable(
                name: "ProductCategoryPicture",
                newName: "ProductCategoryPictures");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategoryPicture_RefId",
                table: "ProductCategoryPictures",
                newName: "IX_ProductCategoryPictures_RefId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategoryPictures",
                table: "ProductCategoryPictures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryPictures_ProductCategories_RefId",
                table: "ProductCategoryPictures",
                column: "RefId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategoryPictures_ProductCategories_RefId",
                table: "ProductCategoryPictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategoryPictures",
                table: "ProductCategoryPictures");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "ProductCategories");

            migrationBuilder.RenameTable(
                name: "ProductCategoryPictures",
                newName: "ProductCategoryPicture");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategoryPictures_RefId",
                table: "ProductCategoryPicture",
                newName: "IX_ProductCategoryPicture_RefId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategoryPicture",
                table: "ProductCategoryPicture",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategoryPicture_ProductCategories_RefId",
                table: "ProductCategoryPicture",
                column: "RefId",
                principalTable: "ProductCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
