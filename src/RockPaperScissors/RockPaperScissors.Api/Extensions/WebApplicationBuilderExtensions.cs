using FluentValidation;
using RockPaperScissors.Api.Contracts.Requests;

namespace RockPaperScissors.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreateGameRequest>();
        builder.Services.AddValidatorsFromAssemblyContaining<JoinGameRequest>();
        builder.Services.AddValidatorsFromAssemblyContaining<MakeTurnRequest>();

        return builder;
    }
}