namespace RockPaperScissors.Api.Services.GameService.Results;

public enum JoinPlayerResult
{
    Success = 0,
    GameNotFound = 1,
    AlreadyMaxPlayersCount = 2,
    PlayerNotFound = 3
}