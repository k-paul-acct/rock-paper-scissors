namespace RockPaperScissors.Api.Services.IdGenerator;

public interface IIdGenerator
{
    public string GeneratePlayerId();

    public string GenerateGameId();
}