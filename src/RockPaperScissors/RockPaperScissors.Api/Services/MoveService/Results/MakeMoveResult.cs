using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Services.MoveService.Results;

public record MakeMoveResult(GameStatus GameStatus, bool PendingOther);