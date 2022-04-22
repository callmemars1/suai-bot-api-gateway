using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using suai_api_schedule.Domain;
using suai_api_schedule.Domain.Exceptions;
using suai_api_schedule.Models;

namespace suai_api_schedule.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        ILogger<ScheduleController> _logger;
        IScheduleProvider _scheduleProvider;
        public ScheduleController(ILogger<ScheduleController> logger, IScheduleProvider scheduleProvider)
        {
            _logger = logger;
            _scheduleProvider = scheduleProvider;
        }

        [HttpGet(Name = "GetSchedule")]
        public IActionResult GetSchedule(string? group = "", string? teacher = "", string? body = "", string? classRoom = "")
        {
            IEnumerable<Lesson> lessons;
            try
            {
                lessons = _scheduleProvider.Get(group, teacher, body, classRoom);
                _logger.Log(LogLevel.Information, $"{lessons.Count()} lessons received");
            }
            catch (ServiceUnavailableException)
            {
                _logger.Log(LogLevel.Warning, "Schedule service unavailable");
                return StatusCode(503);
            }
            return new JsonResult(lessons);
        }

    }
}
