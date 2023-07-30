using FluentValidation;
using RockPaperScissors.Api.Contracts;

namespace RockPaperScissors.Api.Extensions;

public static class ValidatorExtensions
{
    public static IResult? ValidateWithResult<T>(this IValidator<T> validator, T model)
    {
        var validationResult = validator.Validate(model);
        if (validationResult.IsValid) return null;

        var errors = validationResult.Errors.Select(x => new FieldError
        {
            Field = x.PropertyName, Message = x.ErrorMessage
        }).ToList();

        var result = new CommonErrorCollection(ErrorCode.BadRequest, "One or more validation errors occurred", errors);
        return Results.BadRequest(result);
    }
}