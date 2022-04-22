using suai_api_schedule.Domain;

namespace suai_api_schedule.Models;

public interface IScheduleProvider
{
    IEnumerable<Lesson> Get(string group = "*", string teacher = "*", string body = "*", string classRoom = "*");
}
