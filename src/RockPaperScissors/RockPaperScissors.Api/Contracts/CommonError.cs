namespace RockPaperScissors.Api.Contracts;

public class CommonError
{
    public CommonError(Enum code, string message = "")
    {
        Error = code.ToString();
        ErrorCode = Convert.ToInt32(code);
        ErrorDescription = message;
    }

    public string Error { get; init; }
    public int ErrorCode { get; init; }
    public string ErrorDescription { get; init; }
}