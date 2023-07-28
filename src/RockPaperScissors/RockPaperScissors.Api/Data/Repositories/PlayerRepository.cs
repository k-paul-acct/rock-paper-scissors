using System.Collections.Concurrent;
using RockPaperScissors.Api.Game;
using RockPaperScissors.Api.Services.IdGenerator;

namespace RockPaperScissors.Api.Data.Repositories;

public class PlayerRepository
{
    private readonly IIdGenerator _idGenerator;
    private readonly ConcurrentDictionary<string, Player> _players = new();
    private readonly ConcurrentDictionary<string, bool> _restartRequests = new();

    public PlayerRepository(IIdGenerator idGenerator)
    {
        _idGenerator = idGenerator;
    }

    public Player Create(string username)
    {
        var player = new Player
        {
            Id = _idGenerator.GeneratePlayerId(),
            Username = username
        };

        _players.TryAdd(player.Id, player);

        return player;
    }

    public bool RequestGameRestart(string playerId)
    {
        if (_players.ContainsKey(playerId))
        {
            _restartRequests.TryAdd(playerId, true);
            return true;
        }

        return false;
    }

    public bool RestartRequestedForAll(string player1Id, string player2Id)
    {
        return _restartRequests.ContainsKey(player1Id) && _restartRequests.ContainsKey(player2Id);
    }

    public bool DeleteRestartRequest(string playerId)
    {
        return _restartRequests.TryRemove(playerId, out _);
    }

    public Player? GetById(string playerId)
    {
        return _players.TryGetValue(playerId, out var player) ? player : null;
    }

    public bool Delete(string playerId)
    {
        _restartRequests.TryRemove(playerId, out _);
        return _players.TryRemove(playerId, out _);
    }
}