namespace GameIdle.Models
{
    public class Planet
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        // Planet muss gekauft werden (Preis in Credits)
        public long UnlockPriceCredits { get; set; }

        // Balancing
        public long BaseUpgradeCost { get; set; } = 10;
        public double CostMultiplier { get; set; } = 1.35;
        public long BaseProductionPerSecond { get; set; } = 1;
    }
}
