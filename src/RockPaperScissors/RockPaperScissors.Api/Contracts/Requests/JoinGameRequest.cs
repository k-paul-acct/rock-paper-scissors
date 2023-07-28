using FluentValidation;

namespace RockPaperScissors.Api.Contracts.Requests;

public record JoinGameRequest(string GameId, string Username);

public class JoinGameValidator : AbstractValidator<JoinGameRequest>
{
    public JoinGameValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(32);
    }
}