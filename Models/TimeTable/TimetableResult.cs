namespace suai_api.Models.Timetable;

public record struct TimetableResult(Domain.Timetable.WeekTypes ActualWeekType, IEnumerable<Domain.Timetable.LessonDto> Lessons)
{
    public static implicit operator (Domain.Timetable.WeekTypes actualWeekType, IEnumerable<Domain.Timetable.LessonDto> lessons)(TimetableResult value)
    {
        return (value.ActualWeekType, value.Lessons);
    }

    public static implicit operator TimetableResult((Domain.Timetable.WeekTypes actualWeekType, IEnumerable<Domain.Timetable.LessonDto> lessons) value)
    {
        return new TimetableResult(value.actualWeekType, value.lessons);
    }
}