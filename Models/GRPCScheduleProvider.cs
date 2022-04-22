using Grpc.Net.Client;
using Suai.Bot.Schedule;
using Grpc.Core;
using suai_api_schedule.Domain.Exceptions;

namespace suai_api_schedule.Models;

public class GRPCScheduleProvider : IScheduleProvider
{
    private string _serviceAddress;
    private int _reconnectCounter = 0;
    private ScheduleProvider.ScheduleProviderClient _client;
    ILogger<GRPCScheduleProvider> _logger;
    public GRPCScheduleProvider(ILogger<GRPCScheduleProvider> logger, string serviceAddress)
    {
        _logger = logger;
        _serviceAddress = serviceAddress;
        var c = GrpcChannel.ForAddress(_serviceAddress);
        _client = new ScheduleProvider.ScheduleProviderClient(c);
    }

    public IEnumerable<Domain.Lesson> Get(string group = "", string teacher = "", string body = "", string classRoom = "")
    {
        _logger.Log(LogLevel.Information, "Get called");
        try
        {
            var schedule = _client.GetSchedule(new ScheduleRequest
            {
                Group = group,
                Teacher = teacher,
                Body = body,
                ClassRoom = classRoom
            });
            _reconnectCounter = 0;
            _logger.Log(LogLevel.Information, "Received schedule from service");
            return schedule.Lessons.AsEnumerable().Select((lesson) =>
            {
                return new Domain.Lesson
                {
                    Group = lesson.Group,
                    Body = lesson.Body,
                    ClassRoom = lesson.ClassRoom,
                    Teacher = lesson.Teacher,
                    WeekDay = (Domain.WeekDays)((int)lesson.WeekDay),
                    WeekType = (Domain.WeekTypes)((int)lesson.WeekType)
                };
            });
        }
        catch (RpcException ex)
        {
            // if we not connected to the service, we trying to reconnect
            Reconnect();
            return Get(group, teacher, body, classRoom);
        }
    }
    private void Reconnect()
    {
        if (_reconnectCounter == 3)
        {
            _logger.Log(LogLevel.Error, "Failed to connect to the Grpc Schedule Service");
            throw new ServiceUnavailableException();
        }

        // trying to reconnect
        _logger.Log(LogLevel.Warning, "Failed to connect to the Grpc Schedule Service. Reconnecting....");
        var channel = GrpcChannel.ForAddress(_serviceAddress);
        _client = new ScheduleProvider.ScheduleProviderClient(channel);
        ++_reconnectCounter;
    }
}
