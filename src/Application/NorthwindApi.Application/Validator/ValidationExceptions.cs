namespace NorthwindApi.Application.Validator;

public class ValidationExceptions : Exception
{
    public ValidationExceptions(string message) : base(message)
    {
    }

    public ValidationExceptions(string message, Exception innerException) : base(message, innerException)
    {
    }
    public static void Requires(bool expected, string errorMessage)
    {
        if (!expected)
        {
            throw new ValidationExceptions(errorMessage);
        }
    }
}