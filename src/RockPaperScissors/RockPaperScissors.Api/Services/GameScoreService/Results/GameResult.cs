using RockPaperScissors.Api.Data.Models;

namespace RockPaperScissors.Api.Services.GameScoreService.Results;

public class GameResult
{
    public IEnumerable<Player> Players { get; set; } = Enumerable.Empty<Player>();
    public IEnumerable<RoundResult> Rounds { get; set; } = Enumerable.Empty<RoundResult>();
}