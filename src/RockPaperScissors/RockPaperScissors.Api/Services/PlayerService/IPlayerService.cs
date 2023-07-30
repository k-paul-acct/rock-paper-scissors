using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.PlayerService.Results;
using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Services.PlayerService;

public interface IPlayerService
{
    public Task<Player?> CreatePlayer(string name, PlayerType type = PlayerType.Normal);
    public Task<RequestGameRestartResult> RequestGameRestart(string gameId, string playerId);
    public Task<bool> RestartRequestedForAll(string gameId);
    public Task DeleteRestartRequests(string gameId);
}