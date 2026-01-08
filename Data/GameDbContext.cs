using GameIdle.Models;
using Microsoft.EntityFrameworkCore;

namespace GameIdle.Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
        {
        }

        public DbSet<PlayerGameState> PlayerGameStates => Set<PlayerGameState>();
        public DbSet<Planet> Planets => Set<Planet>();
        public DbSet<PlayerPlanetState> PlayerPlanetStates => Set<PlayerPlanetState>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pro Spieler darf es jeden Planeten nur 1x geben
            modelBuilder.Entity<PlayerPlanetState>()
                .HasIndex(x => new { x.PlayerGameStateId, x.PlanetId })
                .IsUnique();

            // Planeten-Seeding (jeder Planet muss gekauft werden!)
            modelBuilder.Entity<Planet>().HasData(
                new Planet
                {
                    Id = 1,
                    Name = "Earth",
                    UnlockPriceCredits = 100,   // ERSTER Planet kostet was
                    BaseUpgradeCost = 10,
                    CostMultiplier = 1.35,
                    BaseProductionPerSecond = 1
                },
                new Planet
                {
                    Id = 2,
                    Name = "Moon",
                    UnlockPriceCredits = 1_000,
                    BaseUpgradeCost = 50,
                    CostMultiplier = 1.40,
                    BaseProductionPerSecond = 3
                },
                new Planet
                {
                    Id = 3,
                    Name = "Mars",
                    UnlockPriceCredits = 25_000,
                    BaseUpgradeCost = 250,
                    CostMultiplier = 1.45,
                    BaseProductionPerSecond = 8
                }
            );
        }
    }
}
