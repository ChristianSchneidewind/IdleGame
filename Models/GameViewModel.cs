using System.Collections.Generic;

namespace GameIdle.Models
{
    public class GameViewModel
    {
        public long Credits { get; set; }
        public long StartCredits { get; set; }
        public long TotalProductionPerSecond { get; set; }

        public long OfflineSeconds { get; set; }      // NEU
        public long OfflineEarnings { get; set; }     // NEU
        public bool ShowOfflinePopup { get; set; }    // NEU

        public List<PlanetViewModel> Planets { get; set; } = new();
    }
}
