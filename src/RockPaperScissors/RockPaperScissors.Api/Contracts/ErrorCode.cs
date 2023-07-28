namespace RockPaperScissors.Api.Contracts;

public enum ErrorCode
{
    GameNotFound = 0,
    PlayerNotFound = 1,
    GameIsFull = 2,
    CannotMakeTurn = 3,
    GameWasNotEnded = 4,
    GameCannotBeRestarted = 5,
    GameIsNotInProcess = 6
}