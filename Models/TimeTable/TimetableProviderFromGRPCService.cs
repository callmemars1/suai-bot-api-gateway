using Suai.Bot.Timetable.Proto;
using suai_api.Domain.TimeTable;

namespace suai_api.Models.Timetable;

public class TimetableProviderFromGRPCService : ITimetableProvider
{
    private readonly TimetableServiceGrpcClient _client;

    private readonly ILogger<TimetableProviderFromGRPCService> _logger;

    public TimetableProviderFromGRPCService(ILogger<TimetableProviderFromGRPCService> logger, string serviceURI)
    {
        _logger = logger;
        _client = new TimetableServiceGrpcClient(serviceURI);
    }

    public TimetableResult GetTimetable(TimetableRequest request)
    {
        var timetable = _client.GetTimetable(request, maxReconnects: 3);

        _logger.Log(LogLevel.Information, "Received timetable from service");

        return new TimetableResult(timetable.ActualWeekType, timetable.Lessons);
    }
}
