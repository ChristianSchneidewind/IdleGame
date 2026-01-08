namespace GameIdle.Models
{
    public class PlayerPlanetState
    {
        public int Id { get; set; }

        public int PlayerGameStateId { get; set; }
        public PlayerGameState PlayerGameState { get; set; } = null!;

        public int PlanetId { get; set; }
        public Planet Planet { get; set; } = null!;

        public bool IsUnlocked { get; set; } = false;

        public int MineLevel { get; set; } = 1;
    }
}
