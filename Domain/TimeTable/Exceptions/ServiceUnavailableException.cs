namespace suai_api.Domain.Timetable.Exceptions;

/// <summary>
/// Ошибка, означающая, что сервис не доступен.
/// Возникает если сервис недоступен или превышено время ожидания ответа
/// </summary>
public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string message = "") : base(message)
    {

    }
}
