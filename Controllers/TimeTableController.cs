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

    // warning выключен для всего метода, т.к. компилятор не понимает, что у string? установлено значение по умолчанию и там невозможен null
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
    [HttpGet]
    [ActionName("get")]
    public IActionResult GetTimeTable(string? group = "", string? teacher = "", string? building = "", string? classRoom = "", bool? test = false)
    {
        _logger.Log(LogLevel.Information, "Params:\n\tg:{group}\n\tt:{teacher}\n\tb:{building}\n\tc:{classRoom}", group, teacher, building, classRoom);

        if (test.HasValue && test == true)
            return new JsonResult(new List<Lesson>()
            {
                new Lesson()
                {
                    Building = building,
                    Group = group,
                    ClassRoom = classRoom,
                    Teacher =teacher,
                    WeekDay = (int)WeekDays.Monday,
                    WeekType = (int)WeekTypes.Upper
                }
            });

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
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

}
