using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameIdle.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePlanets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Planets",
                columns: new[] { "Id", "BaseProductionPerSecond", "BaseUpgradeCost", "CostMultiplier", "Name", "UnlockPriceCredits" },
                values: new object[,]
                {
                    { 4, 22L, 1200L, 1.4199999999999999, "Venus", 250000L },
                    { 5, 70L, 6000L, 1.47, "Mercury", 2500000L },
                    { 6, 220L, 30000L, 1.5, "Jupiter", 25000000L },
                    { 7, 700L, 160000L, 1.52, "Saturn", 250000000L },
                    { 8, 2000L, 900000L, 1.54, "Uranus", 2500000000L },
                    { 9, 6500L, 5000000L, 1.55, "Neptune", 25000000000L },
                    { 10, 22000L, 28000000L, 1.5700000000000001, "Pluto", 250000000000L },
                    { 11, 75000L, 160000000L, 1.5800000000000001, "Ceres", 2500000000000L },
                    { 12, 250000L, 900000000L, 1.6000000000000001, "Eris", 25000000000000L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Planets",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
