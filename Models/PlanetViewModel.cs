namespace GameIdle.Models
{
    public class PlanetViewModel
    {
        public int PlanetId { get; set; }
        public string Name { get; set; } = "";

        public bool IsUnlocked { get; set; }
        public long UnlockPriceCredits { get; set; }

        public int MineLevel { get; set; }
        public long ProductionPerSecond { get; set; }

        public long UpgradeCost1 { get; set; }
        public long UpgradeCost5 { get; set; }
        public long UpgradeCost10 { get; set; }
    }
}
