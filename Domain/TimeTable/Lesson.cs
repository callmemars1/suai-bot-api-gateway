namespace suai_api.Domain.Timetable;

public record class Lesson
{
    public IEnumerable<string> Groups { get; set; }
    public string Teacher { get; set; } = string.Empty;
    public string ClassRoom { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public int OrderNumber { get; set; }
    public WeekDays WeekDay { get; set; }
    public WeekTypes WeekType { get; set; }
    public LessonTypes Type { get; set; }
}
