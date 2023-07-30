using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data;
using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.GameScoreService.Results;
using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Services.GameScoreService;

public class GameScoreService : IGameScoreService
{
    private readonly GameContext _gameContext;

    public GameScoreService(GameContext gameContext)
    {
        _gameContext = gameContext;
    }

    public async Task<Result<GameResult, Error>> GetGameResult(string gameId)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null)
            return Result<GameResult, Error>.FromError(Error.GameNotFound);
        if (game.Status != GameStatus.Ended)
            return Result<GameResult, Error>.FromError(Error.GameIsNotEnded);

        var rounds = await GetRounds(gameId).ToListAsync();
        var players = await GetGamePlayers(gameId);
        var result = new GameResult
        {
            Players = players,
            Rounds = rounds
        };

        return Result<GameResult, Error>.FromOk(result);
    }

    public async Task<Result<GameLive, Error>> GetGameLive(string gameId)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null)
            return Result<GameLive, Error>.FromError(Error.GameNotFound);
        if (game.Status != GameStatus.InProcess)
            return Result<GameLive, Error>.FromError(Error.GameIsNotInProcess);

        var rounds = (await GetRounds(gameId).ToListAsync())
            .Where(x => x.Moves.Count() == Game.MaxPlayers);
        var players = await GetGamePlayers(gameId);

        return Result<GameLive, Error>.FromOk(new GameLive
        {
            CurrentRound = game.RoundsPassed + 1,
            Players = players,
            Rounds = rounds
        });
    }

    public async Task<IEnumerable<Player>> UpdatePlayersScore(Game game)
    {
        var moves = await _gameContext.Moves
            .Where(x => x.GameId == game.Id && x.RoundIndex == game.RoundsPassed)
            .ToListAsync();
        var players = (await GetGamePlayers(game.Id)).ToList();

        var first = moves[0].MoveType;
        var second = moves[1].MoveType;

        var (firstPoints, secondPoints) = (first, second) switch
        {
            (MoveType.Rock, MoveType.Paper) => (0, 2),
            (MoveType.Rock, MoveType.Scissors) => (2, 0),
            (MoveType.Paper, MoveType.Rock) => (2, 0),
            (MoveType.Paper, MoveType.Scissors) => (0, 2),
            (MoveType.Scissors, MoveType.Rock) => (0, 2),
            (MoveType.Scissors, MoveType.Paper) => (2, 0),
            _ => (1, 1)
        };

        players.Find(x => x.Id == moves[0].PlayerId)!.Score += firstPoints;
        players.Find(x => x.Id == moves[1].PlayerId)!.Score += secondPoints;

        await _gameContext.SaveChangesAsync();

        return players;
    }

    private IQueryable<RoundResult> GetRounds(string gameId)
    {
        return _gameContext.Moves
            .Where(x => x.GameId == gameId)
            .OrderBy(x => x.RoundIndex)
            .GroupBy(x => x.RoundIndex, x => x,
                (key, group) => new RoundResult { RoundNumber = key + 1, Moves = group.ToList() });
    }

    private async Task<IEnumerable<Player>> GetGamePlayers(string gameId)
    {
        return await _gameContext.Players.Where(x => x.GameId == gameId).ToListAsync();
    }
}