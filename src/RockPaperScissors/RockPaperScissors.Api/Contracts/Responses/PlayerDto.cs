using RockPaperScissors.Api.Data.Models;

namespace RockPaperScissors.Api.Contracts.Responses;

public record PlayerDto(string Id, string Name, int Score)
{
    public static IEnumerable<PlayerDto> Map(IEnumerable<Player> players)
    {
        return players.Select(x => new PlayerDto(x.Id, x.Name, x.Score));
    }
}