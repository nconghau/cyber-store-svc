using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetApiPostgres.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(24)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    IconUrl = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(24)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    ImgUrl = table.Column<string>(type: "varchar(255)", nullable: false),
                    ImgUrls = table.Column<string>(type: "json", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Tags = table.Column<string>(type: "json", nullable: false),
                    Brand = table.Column<string>(type: "varchar(50)", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Properties = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
