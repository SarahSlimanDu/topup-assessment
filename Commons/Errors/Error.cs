
namespace Commons.Errors;
public record Error(string Code, string Description, ErrorTypes? type)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorTypes.Validation);

    public static implicit operator Result(Error error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}
