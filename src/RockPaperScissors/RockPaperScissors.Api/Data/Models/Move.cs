using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Data.Models;

public class Move
{
    public long Id { get; set; }
    public int RoundIndex { get; set; }
    public string GameId { get; set; } = null!;
    public string PlayerId { get; set; } = null!;
    public MoveType MoveType { get; set; }

    public Game Game { get; set; } = null!;
    public Player Player { get; set; } = null!;
}