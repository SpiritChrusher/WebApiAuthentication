namespace WebApiAuthentication.Exceptions;
public class ValidationFail : Exception
{
    public ValidationFail()
    {
    }

    public ValidationFail(string message) : base(message)
    {
    }
}
