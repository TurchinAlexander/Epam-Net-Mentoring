using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class InsertData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 1, "Fruits", "It's fruits!'" },
                    { 2, "Vegetables", "It's vegetables'!'" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "RegionId", "RegionDescription" },
                values: new object[,]
                {
                    { 1, "New region" },
                    { 2, "Another region" }
                });

            migrationBuilder.InsertData(
                table: "Territories",
                columns: new[] { "TerritoryId", "RegionId", "TerritoryDescription" },
                values: new object[] { 1, 1, "New territory" });

            migrationBuilder.InsertData(
                table: "Territories",
                columns: new[] { "TerritoryId", "RegionId", "TerritoryDescription" },
                values: new object[] { 2, 2, "Another territory" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Territories",
                keyColumn: "TerritoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Territories",
                keyColumn: "TerritoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "RegionId",
                keyValue: 2);
        }
    }
}
