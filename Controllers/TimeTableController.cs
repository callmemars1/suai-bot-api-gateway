using Microsoft.AspNetCore.Mvc;
using suai_api_schedule.Domain.TimeTable.Exceptions;
using suai_api_schedule.Models;

namespace suai_api_schedule.Controllers
{
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
        public IActionResult GetTimeTable(string? group = "", string? teacher = "", string? building = "", string? classRoom = "")
        {
            _logger.Log(LogLevel.Information, "\n\tg:{group}\n\tt:{teacher}\n\tb:{building}\n\tc:{classRoom}", group, teacher, building, classRoom);
            
            IEnumerable<Domain.TimeTable.Lesson> lessons;
            // Попытка получить данные о расписании от сервиса
            try
            {
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                lessons = _timeTableProvider.GetTimeTable(group, teacher, building, classRoom);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                _logger.Log(LogLevel.Information, "{count} lessons received", lessons.Count());
            }
            // Если сервис недоступен
            catch (ServiceUnavailableException)
            {
                _logger.Log(LogLevel.Warning, "TimeTable service unavailable");
                return StatusCode(503);
            }
            return new JsonResult(lessons);
        }

    }
}
