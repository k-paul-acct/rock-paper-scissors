using FluentValidation;
using RockPaperScissors.Api.Contracts;
using RockPaperScissors.Api.Contracts.Requests;
using RockPaperScissors.Api.Contracts.Responses;
using RockPaperScissors.Api.Services.GameScoreService;
using RockPaperScissors.Api.Services.GameService;
using RockPaperScissors.Api.Services.MoveService;
using RockPaperScissors.Api.Services.PlayerService;
using RockPaperScissors.Api.Services.PlayerService.Results;
using RockPaperScissors.Api.Types;
using Serilog;

namespace RockPaperScissors.Api.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();
        return app;
    }

    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/game/create", async (CreateGameRequest model, IValidator<CreateGameRequest> validator,
            IGameService gameService, IPlayerService playerService) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            var game = await gameService.CreateGame();
            if (game is null) return Results.Problem();
            var player = await playerService.CreatePlayer(model.Name);
            if (player is null) return Results.Problem();

            await gameService.JoinPlayer(game.Id, player.Id);

            return Results.Json(new CreateGameDto
            {
                GameId = game.Id,
                PlayerId = player.Id
            }, statusCode: StatusCodes.Status201Created);
        });

        app.MapPost("/game/join", async (JoinGameRequest model, IValidator<JoinGameRequest> validator,
            IGameService gameService, IPlayerService playerService) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            var game = await gameService.GetById(model.GameId);
            if (game is null)
                return Results.NotFound(new CommonError(ErrorCode.GameNotFound));

            if (game.Status != GameStatus.PendingPlayers)
                return Results.BadRequest(new CommonError(ErrorCode.GameIsFull));

            var player = await playerService.CreatePlayer(model.Name);
            if (player is null) return Results.Problem();

            await gameService.JoinPlayer(game.Id, player.Id);

            return Results.Ok(new { PlayerId = player.Id });
        });

        app.MapPost("/game/turn", async (MakeTurnRequest model, IValidator<MakeTurnRequest> validator,
            IGameService gameService, IPlayerService playerService, IMoveService moveService) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            _ = Enum.TryParse(model.Move, out MoveType moveType);

            var result = await moveService.TryMakeMove(model.GameId, model.PlayerId, moveType);
            if (!result.IsOk) return Results.BadRequest(new CommonError(result.Error));

            return Results.Ok(new { GameStatus = result.Ok.GameStatus.ToString(), result.Ok.PendingOther });
        });

        app.MapGet("/game/{gameId}/stat", async (string gameId, IGameScoreService scoreService) =>
        {
            var result = await scoreService.GetGameResult(gameId);
            if (!result.IsOk) return Results.BadRequest(new CommonError(result.Error));
            return Results.Ok(GameResultDto.Map(result.Ok));
        });

        app.MapPost("/game/restart", async (RestartGameRequest model, IValidator<RestartGameRequest> validator,
            IPlayerService playerService, IGameService gameService) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            var result = await playerService.RequestGameRestart(model.GameId, model.PlayerId);
            if (result != RequestGameRestartResult.Success)
                return Results.BadRequest(new CommonError(result));

            if (!await playerService.RestartRequestedForAll(model.GameId))
                return Results.Ok(new { GameRestarted = false, PendingOther = true });

            await gameService.Restart(model.GameId);
            await playerService.DeleteRestartRequests(model.GameId);

            return Results.Ok(new { GameRestarted = true, PendingOther = false });
        });

        app.MapPost("/game/leave", async (LeaveGameRequest model, IValidator<LeaveGameRequest> validator,
            IGameService gameService) =>
        {
            var errors = validator.ValidateWithResult(model);
            if (errors is not null) return errors;

            var result = await gameService.LeavePlayer(model.GameId, model.PlayerId);
            return result ? Results.Ok() : Results.BadRequest();
        });

        app.MapGet("/game/{gameId}/live", async (string gameId, IGameScoreService scoreService) =>
        {
            var result = await scoreService.GetGameLive(gameId);
            if (!result.IsOk) return Results.BadRequest(new CommonError(result.Error));
            return Results.Ok(GameLiveDto.Map(result.Ok));
        });

        return app;
    }
}