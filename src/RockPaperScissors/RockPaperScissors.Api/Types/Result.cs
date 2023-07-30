namespace RockPaperScissors.Api.Types;

public readonly struct Result<TOk, TError>
{
    public readonly TOk Ok;
    public readonly TError Error;
    public bool IsOk { get; }

    private Result(TOk ok, TError error, bool isOk)
    {
        Ok = ok;
        Error = error;
        IsOk = isOk;
    }

    public static Result<TOk, TError> FromOk(TOk ok)
    {
        return new Result<TOk, TError>(ok, default!, true);
    }

    public static Result<TOk, TError> FromError(TError error)
    {
        return new Result<TOk, TError>(default!, error, false);
    }
}