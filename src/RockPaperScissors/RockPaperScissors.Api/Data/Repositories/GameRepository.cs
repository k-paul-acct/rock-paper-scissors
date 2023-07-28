using System.Collections.Concurrent;
using RockPaperScissors.Api.Services.IdGenerator;

namespace RockPaperScissors.Api.Data.Repositories;

public class GameRepository
{
    private readonly ConcurrentDictionary<string, Game.Game> _games = new();
    private readonly IIdGenerator _idGenerator;

    public GameRepository(IIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public Game.Game Create()
    {
        var game = new Game.Game
        {
            Id = _idGenerator.GenerateGameId()
        };

        _games.TryAdd(game.Id, game);

        return game;
    }

    public Game.Game? GetById(string gameId)
    {
        return _games.TryGetValue(gameId, out var game) ? game : null;
    }

    public bool Delete(string gameId)
    {
        return _games.TryRemove(gameId, out _);
    }
}