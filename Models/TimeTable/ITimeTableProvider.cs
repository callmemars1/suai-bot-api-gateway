using suai_api.Domain.TimeTable;

namespace suai_api.Models.TimeTable;

public interface ITimeTableProvider
{
    IEnumerable<Lesson> GetTimeTable(string group = "", string teacher = "", string building = "", string classRoom = "");
}
