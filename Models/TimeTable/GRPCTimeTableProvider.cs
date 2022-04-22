using Grpc.Net.Client;
using Grpc.Core;
using Suai.Bot.TimeTable.Proto;
using suai_api_schedule.Domain.TimeTable.Exceptions;

namespace suai_api_schedule.Models.TimeTable;

public class GRPCTimeTableProvider : ITimeTableProvider
{
    private readonly string _serviceAddress;
    private int _reconnectCounter = 0;
    private TimeTableProvider.TimeTableProviderClient _client;
    private readonly ILogger<GRPCTimeTableProvider> _logger;
    public GRPCTimeTableProvider(ILogger<GRPCTimeTableProvider> logger, string serviceAddress)
    {
        _logger = logger;
        _serviceAddress = serviceAddress;
        var c = GrpcChannel.ForAddress(_serviceAddress);
        _client = new TimeTableProvider.TimeTableProviderClient(c);
    }

    public IEnumerable<Domain.TimeTable.Lesson> GetTimeTable(string group = "", string teacher = "", string building = "", string classRoom = "")
    {
        try
        {
            var schedule = _client.GetTimeTable(new TimeTableRequest
            {
                Group = group,
                Teacher = teacher,
                Building = building,
                ClassRoom = classRoom
            });
            _reconnectCounter = 0;
            _logger.Log(LogLevel.Information, "Received timetable from service");
            return schedule.Lessons.AsEnumerable().Select((lesson) =>
            {
                return new Domain.TimeTable.Lesson
                {
                    Group = lesson.Group,
                    Building = lesson.Building,
                    ClassRoom = lesson.ClassRoom,
                    Teacher = lesson.Teacher,
                    WeekDay = (Domain.TimeTable.WeekDays)((int)lesson.WeekDay),
                    WeekType = (Domain.TimeTable.WeekTypes)((int)lesson.WeekType)
                };
            });
        }
        catch (RpcException)
        {
            // if we not connected to the service, we trying to reconnect
            Reconnect();
            return GetTimeTable(group, teacher, building, classRoom);
        }
    }
    private void Reconnect()
    {
        if (_reconnectCounter == 3)
        {
            throw new ServiceUnavailableException();
        }

        // trying to reconnect
        var channel = GrpcChannel.ForAddress(_serviceAddress);
        _client = new TimeTableProvider.TimeTableProviderClient(channel);
        ++_reconnectCounter;
    }
}
