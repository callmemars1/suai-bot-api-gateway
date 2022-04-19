namespace suai_api_schedule.Domain.Exceptions;

public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message = "") : base(message)
    {

    }
}
