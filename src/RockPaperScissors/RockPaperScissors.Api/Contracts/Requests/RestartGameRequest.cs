namespace RockPaperScissors.Api.Contracts.Requests;

public record RestartGameRequest(string GameId, string PlayerId);