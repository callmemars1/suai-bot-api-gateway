namespace suai_api.Models.Timetable;

public record class TimetableRequestArgs
{
    public string? Group { get; set; }
    public string? Building { get; set; }
    public string? ClassRoom { get; set; }
    public string? Teacher { get; set; }
}
