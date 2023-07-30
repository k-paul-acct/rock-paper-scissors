using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.GameService.Results;

namespace RockPaperScissors.Api.Services.GameService;

public interface IGameService
{
    public Task<Game?> CreateGame();

    public Task<Game?> GetById(string id);

    public Task<JoinPlayerResult> JoinPlayer(string gameId, string playerId);

    public Task Restart(string gameId);

    public Task<bool> LeavePlayer(string gameId, string playerId);
}