using Suai.Bot.Timetable.Proto;
using suai_api.Domain.TimeTable;

namespace suai_api.Models.Timetable;

/// <summary>
/// Класс необходим для получения расписания с сервиса
/// по gRPC
/// </summary>
public class TimetableProviderFromGRPCService : ITimetableProvider
{
    private readonly TimetableServiceGrpcClient _client;

    private readonly ILogger<TimetableProviderFromGRPCService> _logger;

    /// <summary>
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceAddress">URI сервиса</param>
    public TimetableProviderFromGRPCService(ILogger<TimetableProviderFromGRPCService> logger, string serviceURI)
    {
        _logger = logger;
        _client = new TimetableServiceGrpcClient(serviceURI);
    }

    public TimetableResult GetTimetable(TimetableRequestArgs requestArgs)
    {
        var timetable = _client.GetTimetable(new TimetableRequest
        {
            Group = requestArgs.Group ?? "",
            Teacher = requestArgs.Teacher ?? "",
            Building = requestArgs.Building ?? "",
            ClassRoom = requestArgs.ClassRoom ?? ""
        }, maxReconnects: 3);

        _logger.Log(LogLevel.Information, "Received timetable from service");

        var actualWeekType = (Domain.Timetable.WeekTypes)(int)timetable.ActualWeekType;

        // маппим lesson, сгенерированный протобафом в lesson, описанный нами
        var lessons = timetable.Lessons.AsEnumerable().Select(MapProtoLessonToDomainLesson);

        return new TimetableResult(actualWeekType, lessons);
    }

    public Domain.Timetable.LessonDto MapProtoLessonToDomainLesson(Lesson lesson)
    {
        return new Domain.Timetable.LessonDto
        {
            Groups = lesson.Groups,
            Building = lesson.Building,
            ClassRoom = lesson.ClassRoom,
            Teacher = lesson.Teacher,
            WeekDay = (Domain.Timetable.WeekDays)(int)lesson.WeekDay,
            WeekType = (Domain.Timetable.WeekTypes)(int)lesson.WeekType,
            Type = (Domain.Timetable.LessonTypes)(int)lesson.Type,
            Name = lesson.Name,
            EndTime = lesson.EndTime,
            OrderNumber = lesson.OrderNumber,
            StartTime = lesson.StartTime,
        };
    }
}
