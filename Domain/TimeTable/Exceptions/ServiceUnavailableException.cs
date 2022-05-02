namespace suai_api.Domain.Timetable.Exceptions;

/// <summary>
/// Ошибка, означающая, что сервис не доступен.
/// Возникает при следующих статусах RpcException:  Unavailable, DeadlineExceeded
/// </summary>
public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message = "") : base(message)
    {

    }
}
