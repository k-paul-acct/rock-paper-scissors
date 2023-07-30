using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data;
using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.IdGenerator;
using RockPaperScissors.Api.Services.PlayerService.Results;
using RockPaperScissors.Api.Types;
using ILogger = Serilog.ILogger;

namespace RockPaperScissors.Api.Services.PlayerService;

public class PlayerService : IPlayerService
{
    private readonly GameContext _gameContext;
    private readonly IIdGenerator _idGenerator;
    private readonly ILogger _logger;

    public PlayerService(IIdGenerator idGenerator, GameContext gameContext, ILogger logger)
    {
        _idGenerator = idGenerator;
        _gameContext = gameContext;
        _logger = logger;
    }

    public async Task<Player?> CreatePlayer(string name)
    {
        var player = new Player
        {
            Id = _idGenerator.GeneratePlayerId(),
            Name = name
        };

        _gameContext.Players.Add(player);

        try
        {
            await _gameContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            _logger.Error("Cannot create player with id={Id}", player.Id);
            return null;
        }

        return player;
    }

    public async Task<RequestGameRestartResult> RequestGameRestart(string gameId, string playerId)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null) return RequestGameRestartResult.GameNotFound;
        if (game.Status != GameStatus.Ended) return RequestGameRestartResult.GameIsNotEnded;

        var player = await _gameContext.Players.FindAsync(playerId);
        if (player is null || player.GameId != gameId) return RequestGameRestartResult.PlayerNotFound;

        if (await _gameContext.RestartRequests.AnyAsync(x => x.GameId == gameId && x.PlayerId == playerId))
            return RequestGameRestartResult.RestartAlreadyRequested;

        var request = new RestartRequest
        {
            GameId = game.Id,
            PlayerId = player.Id
        };
        _gameContext.RestartRequests.Add(request);
        await _gameContext.SaveChangesAsync();

        return RequestGameRestartResult.Success;
    }

    public async Task<bool> RestartRequestedForAll(string gameId)
    {
        return await _gameContext.RestartRequests.CountAsync(x => x.GameId == gameId) == Game.MaxPlayers;
    }

    public async Task DeleteRestartRequests(string gameId)
    {
        var requests = _gameContext.RestartRequests.Where(x => x.GameId == gameId);
        _gameContext.RestartRequests.RemoveRange(requests);
        await _gameContext.SaveChangesAsync();
    }
}