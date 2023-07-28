using RockPaperScissors.Api.Game;
using RockPaperScissors.Api.Types.Enums;

namespace RockPaperScissors.Api.Contracts.Responses;

public class GameResultResponse
{
    public PlayerResultResponse[] Players { get; set; } = Array.Empty<PlayerResultResponse>();

    private static string[] MapTurns(IEnumerable<Turn> turns)
    {
        return turns.Select(x => x.ToString()).ToArray();
    }

    public static GameResultResponse Map(GameResult gameResult)
    {
        return new GameResultResponse
        {
            Players = gameResult.Players
                .Select(x =>
                    new PlayerResultResponse(x.Player.Username, MapTurns(x.Turns), x.Points, x.Status.ToString()))
                .ToArray()
        };
    }
}

public record PlayerResultResponse(string Username, string[] Turns, int Points, string Status);