namespace RockPaperScissors.Api.Contracts.Requests;

public record LeaveGameRequest(string GameId, string PlayerId);