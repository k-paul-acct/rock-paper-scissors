using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Data.Models;

public class Player
{
    public string Id { get; set; } = null!;
    public string? GameId { get; set; }
    public string Name { get; set; } = null!;
    public int Score { get; set; }
    public PlayerType Type { get; set; }

    public Game Game { get; set; } = null!;
    public ICollection<Move> Moves { get; set; } = new List<Move>();
}