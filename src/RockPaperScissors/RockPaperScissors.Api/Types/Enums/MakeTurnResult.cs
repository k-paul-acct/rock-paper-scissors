namespace RockPaperScissors.Api.Types.Enums;

public enum MakeTurnResult
{
    Success = 0,
    AlreadyMade = 1,
    InvalidTurn = 2,
    PendingPlayers = 3,
    GameAborted = 4,
    GameEnded = 5,
    NoSuchPlayer = 6
}