using RockPaperScissors.Api.Game;

namespace RockPaperScissors.Api.Contracts.Responses;

public class GameLiveResponse
{
    public int CurrentRound { get; set; }

    public PlayerLiveResponse[] Players { get; set; } = Array.Empty<PlayerLiveResponse>();


    private static PlayerLiveResponse[] MapPlayersLive(IEnumerable<PlayerLive> playersLive)
    {
        return playersLive
            .Select(x => new PlayerLiveResponse(x.Player.Username, x.Turns.Select(y => y.ToString()).ToArray()))
            .ToArray();
    }

    public static GameLiveResponse Map(GameLive gameLive)
    {
        return new GameLiveResponse
        {
            CurrentRound = gameLive.CurrentRound,
            Players = MapPlayersLive(gameLive.Players)
        };
    }
}

public record PlayerLiveResponse(string Username, string[] Turns);