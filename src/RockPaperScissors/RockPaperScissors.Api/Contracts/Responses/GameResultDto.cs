using RockPaperScissors.Api.Services.GameScoreService.Results;

namespace RockPaperScissors.Api.Contracts.Responses;

public class GameResultDto
{
    public IEnumerable<PlayerDto> Players { get; set; } = Enumerable.Empty<PlayerDto>();
    public IEnumerable<RoundDto> Rounds { get; set; } = Enumerable.Empty<RoundDto>();

    public static GameResultDto Map(GameResult gameResult)
    {
        return new GameResultDto
        {
            Players = PlayerDto.Map(gameResult.Players),
            Rounds = RoundDto.Map(gameResult.Rounds)
        };
    }
}