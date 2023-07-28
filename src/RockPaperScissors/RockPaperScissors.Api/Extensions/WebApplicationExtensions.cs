using FluentValidation;
using RockPaperScissors.Api.Contracts;
using RockPaperScissors.Api.Contracts.Requests;
using RockPaperScissors.Api.Contracts.Responses;
using RockPaperScissors.Api.Data.Repositories;
using RockPaperScissors.Api.Game;
using RockPaperScissors.Api.Types.Enums;

namespace RockPaperScissors.Api.Extensions;

public static class WebApplicationExtensions
{
    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/game/create", (CreateGameRequest model, IValidator<CreateGameRequest> validator,
            GameRepository gameRepository, PlayerRepository playerRepository) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            var game = gameRepository.Create();
            var player = playerRepository.Create(model.Username);
            game.AddPlayer(player);

            return Results.Json(new CreateGameResponse
            {
                GameId = game.Id,
                PlayerId = player.Id
            }, statusCode: StatusCodes.Status201Created);
        });

        app.MapPost("/game/join", (JoinGameRequest model, IValidator<JoinGameRequest> validator,
            GameRepository gameRepository, PlayerRepository playerRepository) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            var game = gameRepository.GetById(model.GameId);
            if (game is null) return Results.NotFound(new CommonError(ErrorCode.GameNotFound));

            if (game.Status != GameStatus.PendingPlayers)
                return Results.BadRequest(new CommonError(ErrorCode.GameIsFull));

            var player = playerRepository.Create(model.Username);
            game.AddPlayer(player);

            return Results.Ok(new { PlayerId = player.Id });
        });

        app.MapPost("/game/turn", (MakeTurnRequest model, IValidator<MakeTurnRequest> validator,
            GameRepository gameRepository, PlayerRepository playerRepository) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            _ = Enum.TryParse(model.Turn, out Turn turn);

            var game = gameRepository.GetById(model.GameId);
            if (game is null) return Results.BadRequest(new CommonError(ErrorCode.GameNotFound));

            var turnResult = game.MakeTurn(model.PlayerId, turn);

            if (turnResult != MakeTurnResult.Success)
                return Results.BadRequest(new CommonError(ErrorCode.CannotMakeTurn));

            return Results.Ok(new { GameEnded = game.Status == GameStatus.Ended });
        });

        app.MapGet("/game/{gameId}/stat", (string gameId, GameRepository gameRepository) =>
        {
            var game = gameRepository.GetById(gameId);
            if (game is null) return Results.BadRequest(new CommonError(ErrorCode.GameNotFound));

            var result = game.GetResult();
            if (result is null) return Results.BadRequest(new CommonError(ErrorCode.GameWasNotEnded));

            return Results.Ok(GameResultResponse.Map(result));
        });

        app.MapPost("/game/restart", (RestartGameRequest model,
            GameRepository gameRepository, PlayerRepository playerRepository) =>
        {
            var game = gameRepository.GetById(model.GameId);
            if (game is null) return Results.BadRequest(new CommonError(ErrorCode.GameNotFound));
            if (game.Status != GameStatus.Ended)
                return Results.BadRequest(new CommonError(ErrorCode.GameCannotBeRestarted));
            if (!game.ContainsPlayer(model.PlayerId))
                return Results.BadRequest(new CommonError(ErrorCode.PlayerNotFound));

            var res = playerRepository.RequestGameRestart(model.PlayerId);
            if (!res) return Results.BadRequest(new CommonError(ErrorCode.PlayerNotFound));

            if (!playerRepository.RestartRequestedForAll(game.Player1!.Id, game.Player2!.Id))
                return Results.Ok(new { GameRestarted = false, PendingOther = true });

            game.Restart();
            playerRepository.DeleteRestartRequest(game.Player1.Id);
            playerRepository.DeleteRestartRequest(game.Player2.Id);

            return Results.Ok(new { GameRestarted = true, PendingOther = false });
        });

        app.MapPost("/game/leave", (LeaveGameRequest model,
            GameRepository gameRepository, PlayerRepository playerRepository) =>
        {
            var game = gameRepository.GetById(model.GameId);
            if (game is null) return Results.BadRequest(new CommonError(ErrorCode.GameNotFound));
            if (!game.ContainsPlayer(model.PlayerId))
                return Results.BadRequest(new CommonError(ErrorCode.PlayerNotFound));

            if (game.Player1 is not null) playerRepository.Delete(game.Player1.Id);
            if (game.Player2 is not null) playerRepository.Delete(game.Player2.Id);

            gameRepository.Delete(game.Id);

            return Results.Ok();
        });

        app.MapGet("/game/{gameId}/live", (string gameId, GameRepository gameRepository) =>
        {
            var game = gameRepository.GetById(gameId);
            if (game is null) return Results.BadRequest(new CommonError(ErrorCode.GameNotFound));
            if (game.Status != GameStatus.InProcess)
                return Results.BadRequest(new CommonError(ErrorCode.GameIsNotInProcess));

            var live = game.GetLive();

            return Results.Ok(GameLiveResponse.Map(live!));
        });

        return app;
    }
}