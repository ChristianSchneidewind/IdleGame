using System.ComponentModel.DataAnnotations;

namespace GameIdle.Models
{
    public class PlayerGameState
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!;

        public ApplicationUser? User { get; set; }

        public long Credits { get; set; }
        public DateTime LastTickUtc { get; set; } = DateTime.UtcNow;

        // Navigation: deine PlayerPlanetStates (bei dir heißt das "Planets")
        public ICollection<PlayerPlanetState> Planets { get; set; } = new List<PlayerPlanetState>();
    }
}
