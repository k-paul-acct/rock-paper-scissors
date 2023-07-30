using FluentValidation;

namespace RockPaperScissors.Api.Contracts.Requests;

public record RestartGameRequest(string GameId, string PlayerId);

public class RestartGameValidator : AbstractValidator<RestartGameRequest>
{
    public RestartGameValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MaximumLength(32);
    }
}