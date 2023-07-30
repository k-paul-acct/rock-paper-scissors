using RockPaperScissors.Api.Services.GameScoreService.Results;

namespace RockPaperScissors.Api.Contracts.Responses;

public record RoundDto(int RoundNumber, IEnumerable<MoveDto> Moves)
{
    public static IEnumerable<RoundDto> Map(IEnumerable<RoundResult> rounds)
    {
        return rounds.Select(x =>
            new RoundDto(x.RoundNumber, x.Moves.Select(y => new MoveDto(y.PlayerId, y.MoveType.ToString()))));
    }
}