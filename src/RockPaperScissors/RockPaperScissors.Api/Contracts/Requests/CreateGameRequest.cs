using FluentValidation;

namespace RockPaperScissors.Api.Contracts.Requests;

public record CreateGameRequest(string Name, bool WithBot);

public class CreateGameValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(32);
    }
}