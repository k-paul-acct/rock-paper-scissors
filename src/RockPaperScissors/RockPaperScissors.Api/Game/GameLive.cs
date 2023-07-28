using RockPaperScissors.Api.Types.Enums;

namespace RockPaperScissors.Api.Game;

public class GameLive
{
    public int CurrentRound { get; set; }

    public PlayerLive[] Players { get; set; } = Array.Empty<PlayerLive>();
}

public record PlayerLive(Player Player, Turn[] Turns);