using FluentValidation;
using RockPaperScissors.Api.Types.Enums;

namespace RockPaperScissors.Api.Contracts.Requests;

public record MakeTurnRequest(string GameId, string PlayerId, string Turn);

public class MakeTurnValidator : AbstractValidator<MakeTurnRequest>
{
    public MakeTurnValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Turn)
            .NotEmpty()
            .IsEnumName(typeof(Turn));
    }
}