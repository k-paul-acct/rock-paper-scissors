using RockPaperScissors.Api.Services.MoveService.Results;
using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Services.MoveService;

public interface IMoveService
{
    public Task<Result<MakeMoveResult, Error>> TryMakeMove(string gameId, string playerId, MoveType moveType);
}