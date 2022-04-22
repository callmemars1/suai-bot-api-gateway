using suai_api_schedule.Domain.TimeTable;

namespace suai_api_schedule.Models.TimeTable;

public interface ITimeTableProvider
{
    IEnumerable<Lesson> GetTimeTable(string group = "", string teacher = "", string building = "", string classRoom = "");
}
