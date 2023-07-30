using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.GameScoreService.Results;
using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Services.GameScoreService;

public interface IGameScoreService
{
    public Task<Result<GameResult, Error>> GetGameResult(string gameId);
    public Task<Result<GameLive, Error>> GetGameLive(string gameId);
    public Task<IEnumerable<Player>> UpdatePlayersScore(Game game);
}