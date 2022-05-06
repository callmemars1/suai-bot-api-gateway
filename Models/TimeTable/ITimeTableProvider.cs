using Suai.Bot.Timetable.Proto;

namespace suai_api.Models.Timetable;

public interface ITimetableProvider
{
    TimetableResult GetTimetable(TimetableRequest request);
}
