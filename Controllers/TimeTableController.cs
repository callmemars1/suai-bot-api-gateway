using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using suai_api.Domain.Timetable;
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
        if (requestArgs == new TimetableRequestArgs())
            return Test();

        _logger.Log(LogLevel.Information, "Passed args: {}", requestArgs);
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

    private JsonResult Test()
    {
        return new JsonResult(new TimetableResult
        {
            ActualWeekType = WeekTypes.Upper,
            Lessons = new List<Lesson>
            {
                new Lesson()
                {
                    Groups = new string[]{ "test_group 1" },
                    Building = "test_building 1",
                    ClassRoom = "test_class_room 1",
                    LessonName = "test_lesson 1",
                    Teacher = "test_teacher 1",
                    WeekDay = WeekDays.Tuesday,
                    LessonType = LessonTypes.Lecture,
                    WeekType = WeekTypes.Upper,
                    OrderNumber = 1,
                    StartTime = "9:30",
                    EndTime = "11:00",
                },
                new Lesson()
                {
                    Groups = new string[]{ "test_group 1", "test_group 2" },
                    Building = "test_building 2",
                    ClassRoom = "test_class_room 2",
                    LessonName = "test_lesson 2",
                    Teacher = "test_teacher 2",
                    WeekDay = WeekDays.Wednesday,
                    LessonType = LessonTypes.Practical,
                    WeekType = WeekTypes.Upper,
                    OrderNumber = 2,
                    StartTime = "11:10",
                    EndTime = "12:40",
                },
                new Lesson()
                {
                    Groups = new string[]{ "test_group 1" },
                    Building = "test_building 1",
                    ClassRoom = "test_class_room 3",
                    LessonName = "test_lesson 1",
                    Teacher = "test_teacher 1",
                    WeekDay = WeekDays.Monday,
                    LessonType = LessonTypes.Laboratory,
                    WeekType = WeekTypes.Lower,
                    OrderNumber = 3,
                    StartTime = "13:00",
                    EndTime = "14:30",
                },
                new Lesson()
                {
                    Groups = new string[]{"test_group 1", "test_group 2" },
                    Building = "test_building 3",
                    ClassRoom = "test_class_room 4",
                    LessonName = "test_lesson 5",
                    Teacher = "test_teacher 3",
                    WeekDay = WeekDays.Friday,
                    LessonType = LessonTypes.Lecture,
                    WeekType = WeekTypes.Lower,
                    OrderNumber = 4,
                    StartTime = "15:00",
                    EndTime = "16:30",
                },
            }
        });
    }
}
