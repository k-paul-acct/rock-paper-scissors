using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Data.Models;

public class Game
{
    public const int MaxPlayers = 2;
    public string Id { get; set; } = null!;
    public GameStatus Status { get; set; } = GameStatus.PendingPlayers;
    public int RoundsPassed { get; set; }
    public int RoundsNumber { get; set; } = 5;
    public ICollection<Player> Players { get; set; } = new List<Player>();
    public ICollection<Move> Moves { get; set; } = new List<Move>();
}