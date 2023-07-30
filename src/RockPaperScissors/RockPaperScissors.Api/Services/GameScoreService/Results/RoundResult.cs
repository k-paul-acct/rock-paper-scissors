using RockPaperScissors.Api.Data.Models;

namespace RockPaperScissors.Api.Services.GameScoreService.Results;

public class RoundResult
{
    public int RoundNumber { get; set; }

    public IEnumerable<Move> Moves { get; set; } = Enumerable.Empty<Move>();
}