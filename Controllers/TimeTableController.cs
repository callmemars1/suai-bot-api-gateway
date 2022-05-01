using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using suai_api.Domain.Timetable.Exceptions;
using suai_api.Models.Timetable;

namespace suai_api.Controllers;

/// <summary>
/// Контроллер апи расписания гуапа
/// </summary>
[ApiController()]
[Route("suai.[controller].[action]")]
public class TimetableController : ControllerBase
{
    // логгер
    private readonly ILogger<TimetableController> _logger;

    // Провайдер расписания
    private readonly ITimetableProvider _timeTableProvider;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger">Логгер</param>
    /// <param name="timeTableProvider">Провайдер расписания</param>
    public TimetableController(ILogger<TimetableController> logger, ITimetableProvider timeTableProvider)
    {
        _logger = logger;
        _timeTableProvider = timeTableProvider;
    }

    /// <summary>
    /// GET-Метод получения расписания
    /// </summary>
    /// <param name="requestArgs">Параметры запроса</param>
    /// <returns>Результат запроса</returns>
    [HttpGet]
    [ActionName("get")]
    public IActionResult GetTimetable([FromQuery] TimetableRequestArgs requestArgs)
    {
        TimetableResult result;
        // Попытка получить данные о расписании от сервиса
        try
        {
            result = _timeTableProvider.GetTimetable(requestArgs);
            _logger.Log(LogLevel.Information, "{} lessons received", result.Lessons.Count());
        }
        // Если сервис недоступен
        catch (ServiceUnavailableException)
        {
            _logger.Log(LogLevel.Warning, "Timetable service unavailable");
            return StatusCode(503);
        }
        // Ошибка процедуры (но сервис доступен)
        catch (RpcException ex) 
        {
            return ex.StatusCode switch
            {
                Grpc.Core.StatusCode.NotFound => StatusCode(404),
                Grpc.Core.StatusCode.DeadlineExceeded => StatusCode(504),
                _ => StatusCode(500),
            };
        }
        return new JsonResult(result);
    }

}
