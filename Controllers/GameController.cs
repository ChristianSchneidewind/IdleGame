using GameIdle.Data;
using GameIdle.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GameIdle.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameDbContext _db;

        public GameController(GameDbContext db)
        {
            _db = db;
        }

        private const int FirstPlanetId = 1;

        private static long UpgradeCostForNextLevel(Planet planet, int currentLevel)
        {
            // Cost for upgrading from currentLevel -> currentLevel+1
            var cost = planet.BaseUpgradeCost * Math.Pow(planet.CostMultiplier, currentLevel - 1);
            return (long)Math.Round(cost);
        }

        private static long TotalUpgradeCost(Planet planet, int currentLevel, int count)
        {
            long total = 0;
            for (int i = 0; i < count; i++)
            {
                total += UpgradeCostForNextLevel(planet, currentLevel + i);
            }
            return total;
        }

        private static long ProductionPerSecond(Planet planet, int mineLevel)
        {
            // Simple: base PPS * level
            return planet.BaseProductionPerSecond * mineLevel;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? throw new InvalidOperationException("User is not authenticated (no NameIdentifier).");
        }

        private async Task<PlayerGameState> GetOrCreateStateAsync()
        {
            var userId = GetUserId();

            var state = await _db.PlayerGameStates
                .Include(p => p.Planets)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            var planets = await _db.Planets.OrderBy(p => p.Id).ToListAsync();

            if (state == null)
            {
                // StartCredits: genug für Planet 1 kaufen + 5 Upgrades
                var firstPlanet = planets.First(p => p.Id == FirstPlanetId);
                var neededFor5Upgrades = TotalUpgradeCost(firstPlanet, currentLevel: 1, count: 5);
                var startCredits = firstPlanet.UnlockPriceCredits + neededFor5Upgrades;

                state = new PlayerGameState
                {
                    UserId = userId,
                    Credits = startCredits,
                    LastTickUtc = DateTime.UtcNow
                };

                _db.PlayerGameStates.Add(state);
                await _db.SaveChangesAsync();

                // PlayerPlanetStates für alle Planeten anlegen (alle angezeigt, aber locked)
                foreach (var pl in planets)
                {
                    _db.PlayerPlanetStates.Add(new PlayerPlanetState
                    {
                        PlayerGameStateId = state.Id,
                        PlanetId = pl.Id,
                        IsUnlocked = false,
                        MineLevel = 1
                    });
                }

                await _db.SaveChangesAsync();
            }
            else
            {
                // Falls Seeding erweitert wurde: fehlende PlayerPlanetStates nachziehen
                foreach (var pl in planets)
                {
                    if (!state.Planets.Any(x => x.PlanetId == pl.Id))
                    {
                        _db.PlayerPlanetStates.Add(new PlayerPlanetState
                        {
                            PlayerGameStateId = state.Id,
                            PlanetId = pl.Id,
                            IsUnlocked = false,
                            MineLevel = 1
                        });
                    }
                }

                await _db.SaveChangesAsync();
            }

            return state;
        }

        private async Task<(long offlineSeconds, long offlineEarnings)> ApplyTickAsync(PlayerGameState state)
        {
            var now = DateTime.UtcNow;
            var deltaSecondsDouble = (now - state.LastTickUtc).TotalSeconds;

            if (deltaSecondsDouble <= 0)
                return (0, 0);

            // Alle gekauften Planeten produzieren parallel
            var owned = await _db.PlayerPlanetStates
                .Where(x => x.PlayerGameStateId == state.Id && x.IsUnlocked)
                .Include(x => x.Planet)
                .ToListAsync();

            long totalPps = 0;
            foreach (var p in owned)
                totalPps += ProductionPerSecond(p.Planet, p.MineLevel);

            var offlineSeconds = (long)deltaSecondsDouble;
            var earned = offlineSeconds * totalPps;

            state.Credits += earned;
            state.LastTickUtc = now;

            return (offlineSeconds, earned);
        }

        public async Task<IActionResult> Index()
        {
            var state = await GetOrCreateStateAsync();

            var (offlineSeconds, offlineEarnings) = await ApplyTickAsync(state);
            await _db.SaveChangesAsync();

            var states = await _db.PlayerPlanetStates
                .Where(x => x.PlayerGameStateId == state.Id)
                .Include(x => x.Planet)
                .OrderBy(x => x.PlanetId)
                .ToListAsync();

            long totalPps = states
                .Where(s => s.IsUnlocked)
                .Sum(s => ProductionPerSecond(s.Planet, s.MineLevel));

            var vm = new GameViewModel
            {
                Credits = state.Credits,
                StartCredits = state.Credits,
                TotalProductionPerSecond = totalPps,

                OfflineSeconds = offlineSeconds,
                OfflineEarnings = offlineEarnings,

                // ✅ DAS war der Bug: Ohne diese Zeile wird das Popup NIE gerendert.
                ShowOfflinePopup = offlineSeconds > 0,

                Planets = states.Select(s => new PlanetViewModel
                {
                    PlanetId = s.PlanetId,
                    Name = s.Planet.Name,
                    IsUnlocked = s.IsUnlocked,
                    UnlockPriceCredits = s.Planet.UnlockPriceCredits,
                    MineLevel = s.MineLevel,
                    ProductionPerSecond = s.IsUnlocked ? ProductionPerSecond(s.Planet, s.MineLevel) : 0,
                    UpgradeCost1 = s.IsUnlocked ? TotalUpgradeCost(s.Planet, s.MineLevel, 1) : 0,
                    UpgradeCost5 = s.IsUnlocked ? TotalUpgradeCost(s.Planet, s.MineLevel, 5) : 0,
                    UpgradeCost10 = s.IsUnlocked ? TotalUpgradeCost(s.Planet, s.MineLevel, 10) : 0
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyPlanet(int planetId)
        {
            var state = await GetOrCreateStateAsync();
            await ApplyTickAsync(state);

            var planet = await _db.Planets.FirstAsync(p => p.Id == planetId);
            var pstate = await _db.PlayerPlanetStates.FirstAsync(x =>
                x.PlayerGameStateId == state.Id && x.PlanetId == planetId);

            if (!pstate.IsUnlocked && state.Credits >= planet.UnlockPriceCredits)
            {
                state.Credits -= planet.UnlockPriceCredits;
                pstate.IsUnlocked = true;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpgradeMine(int planetId, int count)
        {
            if (count != 1 && count != 5 && count != 10)
                return RedirectToAction(nameof(Index));

            var state = await GetOrCreateStateAsync();
            await ApplyTickAsync(state);

            var pstate = await _db.PlayerPlanetStates
                .Include(x => x.Planet)
                .FirstAsync(x => x.PlayerGameStateId == state.Id && x.PlanetId == planetId);

            if (!pstate.IsUnlocked)
                return RedirectToAction(nameof(Index));

            var cost = TotalUpgradeCost(pstate.Planet, pstate.MineLevel, count);

            if (state.Credits >= cost)
            {
                state.Credits -= cost;
                pstate.MineLevel += count;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> State()
        {
            var state = await GetOrCreateStateAsync();
            await ApplyTickAsync(state);
            await _db.SaveChangesAsync();

            var planetStates = await _db.PlayerPlanetStates
                .Where(x => x.PlayerGameStateId == state.Id)
                .Include(x => x.Planet)
                .OrderBy(x => x.PlanetId)
                .ToListAsync();

            long totalPps = planetStates
                .Where(s => s.IsUnlocked)
                .Sum(s => ProductionPerSecond(s.Planet, s.MineLevel));

            var payload = new
            {
                credits = state.Credits,
                totalPps = totalPps,
                planets = planetStates.Select(s => new
                {
                    planetId = s.PlanetId,
                    isUnlocked = s.IsUnlocked,
                    mineLevel = s.MineLevel,
                    unlockPrice = s.Planet.UnlockPriceCredits,
                    pps = s.IsUnlocked ? ProductionPerSecond(s.Planet, s.MineLevel) : 0,
                    cost1 = s.IsUnlocked ? TotalUpgradeCost(s.Planet, s.MineLevel, 1) : 0,
                    cost5 = s.IsUnlocked ? TotalUpgradeCost(s.Planet, s.MineLevel, 5) : 0,
                    cost10 = s.IsUnlocked ? TotalUpgradeCost(s.Planet, s.MineLevel, 10) : 0
                })
            };

            return Json(payload);
        }
    }
}
