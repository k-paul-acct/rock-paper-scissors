namespace RockPaperScissors.Api.Contracts;

public class CommonError
{
    public CommonError(ErrorCode code, string message = "")
    {
        Error = code.ToString();
        ErrorCode = code;
        ErrorDescription = message;
    }

    public string Error { get; init; }
    public ErrorCode ErrorCode { get; init; }
    public string ErrorDescription { get; init; }
}