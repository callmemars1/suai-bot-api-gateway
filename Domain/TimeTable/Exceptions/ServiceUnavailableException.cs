namespace suai_api_schedule.Domain.TimeTable.Exceptions;

public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message = "") : base(message)
    {

    }
}
