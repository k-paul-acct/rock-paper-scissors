namespace RockPaperScissors.Api.Contracts;

public enum ErrorCode
{
    BadRequest = 0,
    GameNotFound = 1,
    PlayerNotFound = 2,
    GameIsFull = 3,
    CannotMakeMove = 4,
    GameWasNotEnded = 5,
    GameCannotBeRestarted = 6,
    GameIsNotInProcess = 7
}