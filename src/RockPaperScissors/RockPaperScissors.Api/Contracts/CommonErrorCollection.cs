namespace RockPaperScissors.Api.Contracts;

public class CommonErrorCollection : CommonError
{
    public CommonErrorCollection(ErrorCode code, string message = "") : base(code, message)
    {
    }

    public CommonErrorCollection(ErrorCode code, string message, IEnumerable<FieldError> fieldErrors)
        : base(code, message)
    {
        FieldErrors = fieldErrors;
    }

    public CommonErrorCollection(ErrorCode code, IEnumerable<FieldError> fieldErrors) : base(code)
    {
        FieldErrors = fieldErrors;
    }

    public IEnumerable<FieldError> FieldErrors { get; set; } = Enumerable.Empty<FieldError>();
}