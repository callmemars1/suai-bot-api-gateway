using suai_api_schedule.Domain;
using suai_api_schedule.Domain.TimeTable;

namespace suai_api_schedule.Models;

public interface ITimeTableProvider
{
    IEnumerable<Lesson> GetTimeTable(string group = "", string teacher = "", string building = "", string classRoom = "");
}
