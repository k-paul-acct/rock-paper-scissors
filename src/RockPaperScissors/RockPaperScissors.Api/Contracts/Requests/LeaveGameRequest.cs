using FluentValidation;

namespace RockPaperScissors.Api.Contracts.Requests;

public record LeaveGameRequest(string GameId, string PlayerId);

public class LeaveGameValidator : AbstractValidator<LeaveGameRequest>
{
    public LeaveGameValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MaximumLength(32);
    }
}