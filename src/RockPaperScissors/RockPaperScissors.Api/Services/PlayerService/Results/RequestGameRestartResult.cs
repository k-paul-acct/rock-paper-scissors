namespace RockPaperScissors.Api.Services.PlayerService.Results;

public enum RequestGameRestartResult
{
    Success = 0,
    GameNotFound = 1,
    GameIsNotEnded = 2,
    PlayerNotFound = 3,
    RestartAlreadyRequested = 4
}