namespace RockPaperScissors.Api.Services.IdGenerator;

public class IdGenerator : IIdGenerator
{
    public string GeneratePlayerId()
    {
        return GenerateId();
    }

    public string GenerateGameId()
    {
        return GenerateId();
    }

    private static string GenerateId()
    {
        Span<byte> bytes = stackalloc byte[9];
        Random.Shared.NextBytes(bytes);
        return Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_');
    }
}