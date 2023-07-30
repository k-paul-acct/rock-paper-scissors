using RockPaperScissors.Api.Services.GameScoreService.Results;

namespace RockPaperScissors.Api.Contracts.Responses;

public class GameLiveDto
{
    public int CurrentRound { get; set; }

    public IEnumerable<PlayerDto> Players { get; set; } = Enumerable.Empty<PlayerDto>();

    public IEnumerable<RoundDto> Rounds { get; set; } = Enumerable.Empty<RoundDto>();

    public static GameLiveDto Map(GameLive gameLive)
    {
        return new GameLiveDto
        {
            CurrentRound = gameLive.CurrentRound,
            Players = PlayerDto.Map(gameLive.Players),
            Rounds = RoundDto.Map(gameLive.Rounds)
        };
    }
}