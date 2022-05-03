namespace suai_api.Models.Timetable;

public interface ITimetableProvider
{
    TimetableResult GetTimetable(TimetableRequestArgs requestArgsArgs);
}
