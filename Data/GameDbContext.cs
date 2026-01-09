using GameIdle.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameIdle.Data
{
    public class GameDbContext : IdentityDbContext<ApplicationUser>
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

            // ✅ 1 Save pro User
            modelBuilder.Entity<PlayerGameState>()
                .HasIndex(x => x.UserId)
                .IsUnique();

            modelBuilder.Entity<PlayerGameState>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Pro Spieler darf es jeden Planeten nur 1x geben
            modelBuilder.Entity<PlayerPlanetState>()
                .HasIndex(x => new { x.PlayerGameStateId, x.PlanetId })
                .IsUnique();

            // Planeten-Seeding (jeder Planet muss gekauft werden!)
            modelBuilder.Entity<Planet>().HasData(
                new Planet { Id = 1,  Name = "Earth",   UnlockPriceCredits = 100,        BaseUpgradeCost = 10,      CostMultiplier = 1.35, BaseProductionPerSecond = 1  },
                new Planet { Id = 2,  Name = "Moon",    UnlockPriceCredits = 1_000,      BaseUpgradeCost = 50,      CostMultiplier = 1.40, BaseProductionPerSecond = 3  },
                new Planet { Id = 3,  Name = "Mars",    UnlockPriceCredits = 25_000,     BaseUpgradeCost = 250,     CostMultiplier = 1.45, BaseProductionPerSecond = 8  },
                new Planet { Id = 4,  Name = "Venus",   UnlockPriceCredits = 250_000,    BaseUpgradeCost = 1_200,   CostMultiplier = 1.42, BaseProductionPerSecond = 22 },

                new Planet { Id = 5,  Name = "Mercury", UnlockPriceCredits = 2_500_000,  BaseUpgradeCost = 6_000,   CostMultiplier = 1.47, BaseProductionPerSecond = 70 },
                new Planet { Id = 6,  Name = "Jupiter", UnlockPriceCredits = 25_000_000, BaseUpgradeCost = 30_000,  CostMultiplier = 1.50, BaseProductionPerSecond = 220 },
                new Planet { Id = 7,  Name = "Saturn",  UnlockPriceCredits = 250_000_000,BaseUpgradeCost = 160_000, CostMultiplier = 1.52, BaseProductionPerSecond = 700 },
                new Planet { Id = 8,  Name = "Uranus",  UnlockPriceCredits = 2_500_000_000, BaseUpgradeCost = 900_000, CostMultiplier = 1.54, BaseProductionPerSecond = 2_000 },

                new Planet { Id = 9,  Name = "Neptune", UnlockPriceCredits = 25_000_000_000, BaseUpgradeCost = 5_000_000,  CostMultiplier = 1.55, BaseProductionPerSecond = 6_500 },
                new Planet { Id = 10, Name = "Pluto",   UnlockPriceCredits = 250_000_000_000, BaseUpgradeCost = 28_000_000, CostMultiplier = 1.57, BaseProductionPerSecond = 22_000 },
                new Planet { Id = 11, Name = "Ceres",   UnlockPriceCredits = 2_500_000_000_000, BaseUpgradeCost = 160_000_000, CostMultiplier = 1.58, BaseProductionPerSecond = 75_000 },
                new Planet { Id = 12, Name = "Eris",    UnlockPriceCredits = 25_000_000_000_000, BaseUpgradeCost = 900_000_000, CostMultiplier = 1.60, BaseProductionPerSecond = 250_000 }


            );
        }
    }
}
