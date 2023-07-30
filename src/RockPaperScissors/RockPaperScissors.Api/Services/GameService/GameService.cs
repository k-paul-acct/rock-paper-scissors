using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data;
using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.GameService.Results;
using RockPaperScissors.Api.Services.IdGenerator;
using RockPaperScissors.Api.Types;
using ILogger = Serilog.ILogger;

namespace RockPaperScissors.Api.Services.GameService;

public class GameService : IGameService
{
    private readonly GameContext _gameContext;
    private readonly IIdGenerator _idGenerator;
    private readonly ILogger _logger;

    public GameService(GameContext gameContext, IIdGenerator idGenerator, ILogger logger)
    {
        _gameContext = gameContext;
        _idGenerator = idGenerator;
        _logger = logger;
    }

    public async Task<Game?> CreateGame()
    {
        var game = new Game
        {
            Id = _idGenerator.GenerateGameId()
        };

        _gameContext.Add(game);

        try
        {
            await _gameContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            _logger.Error("Cannot create game with id={Id}", game.Id);
            return null;
        }

        return game;
    }

    public async Task<Game?> GetById(string id)
    {
        return await _gameContext.Games.FindAsync(id);
    }

    public async Task<JoinPlayerResult> JoinPlayer(string gameId, string playerId)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null) return JoinPlayerResult.GameNotFound;
        var player = await _gameContext.Players.FindAsync(playerId);
        if (player is null) return JoinPlayerResult.PlayerNotFound;

        var count = await _gameContext.Players.CountAsync(x => x.GameId == gameId);
        if (count >= Game.MaxPlayers) return JoinPlayerResult.AlreadyMaxPlayersCount;

        player.GameId = gameId;

        if (count + 1 == Game.MaxPlayers) game.Status = GameStatus.InProcess;

        await _gameContext.SaveChangesAsync();

        return JoinPlayerResult.Success;
    }

    public async Task Restart(string gameId)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null) return;

        game.Status = GameStatus.InProcess;
        game.RoundsPassed = 0;

        await _gameContext.Players.Where(x => x.GameId == gameId).ForEachAsync(x => x.Score = 0);

        var moves = _gameContext.Moves.Where(x => x.GameId == gameId);
        _gameContext.Moves.RemoveRange(moves);

        await _gameContext.SaveChangesAsync();
    }

    public async Task<bool> LeavePlayer(string gameId, string playerId)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null) return false;

        var player = await _gameContext.Players.FindAsync(playerId);
        if (player is null || player.GameId != gameId) return false;

        _gameContext.Players.Remove(player);
        game.Status = GameStatus.Aborted;
        await _gameContext.SaveChangesAsync();

        if (await _gameContext.Players.CountAsync(x => x.GameId == gameId) == 0)
        {
            _gameContext.Games.Remove(game);
            await _gameContext.SaveChangesAsync();
        }

        return true;
    }
}