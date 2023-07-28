using FluentValidation;

namespace RockPaperScissors.Api.Contracts.Requests;

public record CreateGameRequest(string Username, bool WithBot);

public class CreateGameValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(32);
    }
}