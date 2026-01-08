using System;
using System.Collections.Generic;

namespace GameIdle.Models
{
    public class PlayerGameState
    {
        public int Id { get; set; }

        public long Credits { get; set; } = 0;

        public DateTime LastTickUtc { get; set; } = DateTime.UtcNow;

        public List<PlayerPlanetState> Planets { get; set; } = new();
    }
}
