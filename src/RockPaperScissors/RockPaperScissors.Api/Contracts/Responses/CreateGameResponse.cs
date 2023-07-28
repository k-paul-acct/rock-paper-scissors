namespace RockPaperScissors.Api.Contracts.Responses;

public class CreateGameResponse
{
    public required string GameId { get; set; }
    public required string PlayerId { get; set; }
}