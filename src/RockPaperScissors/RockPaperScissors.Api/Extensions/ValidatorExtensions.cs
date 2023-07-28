using FluentValidation;

namespace RockPaperScissors.Api.Extensions;

public static class ValidatorExtensions
{
    public static IResult? ValidateWithResult<T>(this IValidator<T> validator, T model)
    {
        var validationResult = validator.Validate(model);
        if (validationResult.IsValid) return null;

        var errors = validationResult.Errors.Select(x => new
        {
            Field = x.PropertyName, Message = x.ErrorMessage
        });

        return Results.BadRequest(new
        {
            Error = "BadRequest", ErrorDescription = "One or more validation errors occurred", Errors = errors
        });
    }
}