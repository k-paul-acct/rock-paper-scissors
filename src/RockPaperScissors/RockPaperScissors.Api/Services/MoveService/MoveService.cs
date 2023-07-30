using Microsoft.EntityFrameworkCore;
using RockPaperScissors.Api.Data;
using RockPaperScissors.Api.Data.Models;
using RockPaperScissors.Api.Services.GameScoreService;
using RockPaperScissors.Api.Services.MoveService.Results;
using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Services.MoveService;

public class MoveService : IMoveService
{
    private readonly GameContext _gameContext;
    private readonly IGameScoreService _scoreService;

    public MoveService(GameContext gameContext, IGameScoreService scoreService)
    {
        _gameContext = gameContext;
        _scoreService = scoreService;
    }

    public async Task<Result<MakeMoveResult, Error>> TryMakeMove(string gameId, string playerId, MoveType moveType)
    {
        var game = await _gameContext.Games.FindAsync(gameId);
        if (game is null)
            return Result<MakeMoveResult, Error>.FromError(Error.GameNotFound);
        if (game.Status != GameStatus.InProcess)
            return Result<MakeMoveResult, Error>.FromError(Error.GameIsNotInProcess);

        var player = await _gameContext.Players.FindAsync(playerId);
        if (player is null || player.GameId != gameId)
            return Result<MakeMoveResult, Error>.FromError(Error.PlayerNotFound);

        if (await _gameContext.Moves.AnyAsync(x =>
                x.PlayerId == playerId &&
                x.GameId == gameId &&
                x.RoundIndex == game.RoundsPassed))
            return Result<MakeMoveResult, Error>.FromError(Error.MoveAlreadyMade);

        return await MakeMove(game, player, moveType);
    }

    private async Task<bool> AllMadeTurn(string gameId, int roundsPassed)
    {
        var movesInRound = await _gameContext.Moves.CountAsync(
            x => x.GameId == gameId && x.RoundIndex == roundsPassed);
        return movesInRound == Game.MaxPlayers;
    }

    private static void TryToEndGame(Game game, IEnumerable<Player> players)
    {
        var limit = game.RoundsNumber / 2 + 1;
        if (players.Any(x => x.Score >= limit)) game.Status = GameStatus.Ended;
        if (game.RoundsPassed == game.RoundsNumber) game.Status = GameStatus.Ended;
    }

    private async Task<Result<MakeMoveResult, Error>> MakeMove(Game game, Player player, MoveType moveType)
    {
        var move = new Move
        {
            PlayerId = player.Id,
            GameId = game.Id,
            RoundIndex = game.RoundsPassed,
            MoveType = moveType
        };

        _gameContext.Moves.Add(move);
        await _gameContext.SaveChangesAsync();

        var allMadeTurn = await AllMadeTurn(game.Id, game.RoundsPassed);
        if (allMadeTurn)
        {
            var players = await _scoreService.UpdatePlayersScore(game);
            ++game.RoundsPassed;
            TryToEndGame(game, players);
            await _gameContext.SaveChangesAsync();
        }

        var result = new MakeMoveResult(game.Status, !allMadeTurn);
        return Result<MakeMoveResult, Error>.FromOk(result);
    }
}