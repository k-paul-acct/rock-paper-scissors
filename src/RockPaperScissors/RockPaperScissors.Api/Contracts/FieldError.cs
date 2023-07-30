namespace RockPaperScissors.Api.Contracts;

public class FieldError
{
    public required string Field { get; set; }
    public required string Message { get; set; }
}