using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameIdle.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanetsPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MineLevel",
                table: "PlayerGameStates");

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnlockPriceCredits = table.Column<long>(type: "bigint", nullable: false),
                    BaseUpgradeCost = table.Column<long>(type: "bigint", nullable: false),
                    CostMultiplier = table.Column<double>(type: "float", nullable: false),
                    BaseProductionPerSecond = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerPlanetStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerGameStateId = table.Column<int>(type: "int", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: false),
                    IsUnlocked = table.Column<bool>(type: "bit", nullable: false),
                    MineLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPlanetStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerPlanetStates_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerPlanetStates_PlayerGameStates_PlayerGameStateId",
                        column: x => x.PlayerGameStateId,
                        principalTable: "PlayerGameStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Planets",
                columns: new[] { "Id", "BaseProductionPerSecond", "BaseUpgradeCost", "CostMultiplier", "Name", "UnlockPriceCredits" },
                values: new object[,]
                {
                    { 1, 1L, 10L, 1.3500000000000001, "Earth", 100L },
                    { 2, 3L, 50L, 1.3999999999999999, "Moon", 1000L },
                    { 3, 8L, 250L, 1.45, "Mars", 25000L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPlanetStates_PlanetId",
                table: "PlayerPlanetStates",
                column: "PlanetId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerPlanetStates_PlayerGameStateId_PlanetId",
                table: "PlayerPlanetStates",
                columns: new[] { "PlayerGameStateId", "PlanetId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerPlanetStates");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.AddColumn<int>(
                name: "MineLevel",
                table: "PlayerGameStates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
