namespace suai_api.Domain.TimeTable.Exceptions;

public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message = "") : base(message)
    {

    }
}
