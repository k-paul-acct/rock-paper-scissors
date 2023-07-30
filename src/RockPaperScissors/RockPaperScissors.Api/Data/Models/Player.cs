namespace RockPaperScissors.Api.Data.Models;

public class Player
{
    public string Id { get; set; } = null!;
    public string? GameId { get; set; }
    public string Name { get; set; } = null!;
    public int Score { get; set; }

    public Game Type { get; set; } = null!;
    public ICollection<Move> Moves { get; set; } = new List<Move>();
}