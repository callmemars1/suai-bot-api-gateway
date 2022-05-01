using suai_api.Domain.Timetable;

namespace suai_api.Models.Timetable;

/// <summary>
/// Единый интерфейс для всех классов, предоставляющих расписание
/// </summary>
public interface ITimetableProvider
{
    TimetableResult GetTimetable(TimetableRequestArgs requestArgsArgs);
}
