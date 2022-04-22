namespace suai_api_schedule.Domain.TimeTable;

public record class Lesson
{
    public string Group { get; set; } = string.Empty;
    public string Teacher { get; set; } = string.Empty;
    public string ClassRoom { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public WeekDays WeekDay { get; set; }
    public WeekTypes WeekType { get; set; }
}
