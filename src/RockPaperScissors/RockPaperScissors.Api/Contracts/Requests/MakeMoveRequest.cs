using FluentValidation;
using RockPaperScissors.Api.Types;

namespace RockPaperScissors.Api.Contracts.Requests;

public record MakeMoveRequest(string GameId, string PlayerId, string Move);

public class MakeTurnValidator : AbstractValidator<MakeMoveRequest>
{
    public MakeTurnValidator()
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.PlayerId)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Move)
            .NotEmpty()
            .IsEnumName(typeof(MoveType));
    }
}