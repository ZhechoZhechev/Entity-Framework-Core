using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetStoreWorkshop.Data.Migrations
{
    /// <inheritdoc />
    public partial class PetProductImagesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Link to an image");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Link to an image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Pets");
        }
    }
}
