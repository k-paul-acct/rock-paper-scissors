using RockPaperScissors.Api.Types.Enums;

namespace RockPaperScissors.Api.Game;

public class GameResult
{
    public PlayerResult[] Players { get; set; } = new PlayerResult[2];
}

public record PlayerResult(Player Player, Turn[] Turns, int Points, PlayerStatus Status);