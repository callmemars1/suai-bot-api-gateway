using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using suai_api_schedule.Domain.TimeTable;
using suai_api_schedule.Domain.TimeTable.Exceptions;
using suai_api_schedule.Models.TimeTable;

namespace suai_api_schedule.Controllers;

[ApiController()]
[Route("api.[controller].[action]")]
public class TimeTableController : ControllerBase
{
    private readonly ILogger<TimeTableController> _logger;
    readonly ITimeTableProvider _timeTableProvider;
    public TimeTableController(ILogger<TimeTableController> logger, ITimeTableProvider scheduleProvider)
    {
        _logger = logger;
        _timeTableProvider = scheduleProvider;
    }

    [HttpGet]
    [ActionName("get")]
    // TODO: Вынести аргументы в отдельную структуру для читаемости,
    // Убрать университет из запроса :(
    public IActionResult GetTimeTable(string? university, string? group, string? teacher, string? building, string? classRoom)
    {
        IEnumerable<Lesson> lessons;
        // Попытка получить данные о расписании от сервиса
        try
        {
            lessons = _timeTableProvider.GetTimeTable(group, teacher, building, classRoom);
            _logger.Log(LogLevel.Information, "{count} lessons received", lessons.Count());
        }
        // Если сервис недоступен
        catch (ServiceUnavailableException)
        {
            _logger.Log(LogLevel.Warning, "TimeTable service unavailable");
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
        return new JsonResult(lessons);
    }

}
