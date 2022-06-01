using Suai.Bot.Timetable.Proto;

namespace suai_api.Models.Timetable;

public record class TimetableResult(WeekTypes ActualWeekType, IEnumerable<Lesson> Lessons)
{
    public static implicit operator (WeekTypes actualWeekType, IEnumerable<Lesson> lessons)(TimetableResult value)
    {
        return (value.ActualWeekType, value.Lessons);
    }

    public static implicit operator TimetableResult((WeekTypes actualWeekType, IEnumerable<Lesson> lessons) value)
    {
        return new TimetableResult(value.actualWeekType, value.lessons);
    }
}