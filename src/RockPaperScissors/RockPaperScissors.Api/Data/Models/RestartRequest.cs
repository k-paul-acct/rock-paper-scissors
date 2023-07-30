namespace RockPaperScissors.Api.Data.Models;

public class RestartRequest
{
    public long Id { get; set; }
    public string PlayerId { get; set; } = null!;
    public string GameId { get; set; } = null!;

    public Player Player { get; set; } = null!;
    public Game Game { get; set; } = null!;
}