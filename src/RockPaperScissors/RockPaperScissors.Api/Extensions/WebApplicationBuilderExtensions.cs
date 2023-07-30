using FluentValidation;
using RockPaperScissors.Api.Contracts.Requests;
using Serilog;

namespace RockPaperScissors.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreateGameRequest>();
        builder.Services.AddValidatorsFromAssemblyContaining<JoinGameRequest>();
        builder.Services.AddValidatorsFromAssemblyContaining<MakeMoveRequest>();
        builder.Services.AddValidatorsFromAssemblyContaining<LeaveGameRequest>();
        builder.Services.AddValidatorsFromAssemblyContaining<RestartGameRequest>();

        return builder;
    }

    public static WebApplicationBuilder SetUpLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(ctx.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name ?? "Program")
                .Enrich.WithProperty("Environment", ctx.HostingEnvironment);
        });

        builder.Services.AddLogging();

        return builder;
    }
}