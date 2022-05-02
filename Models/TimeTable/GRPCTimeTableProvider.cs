using Grpc.Net.Client;
using Grpc.Core;
using Suai.Bot.Timetable.Proto;
using suai_api.Domain.Timetable.Exceptions;

namespace suai_api.Models.Timetable;

/// <summary>
/// Класс необходим для получения расписания с сервиса
/// по gRPC
/// </summary>
public class GRPCTimetableProvider : ITimetableProvider
{
    // адрес сервиса
    private readonly string _serviceAddress;

    // Счетчик реконектов
    private int _reconnectCounter = 0;

    // gRPC клиент 
    private TimetableProvider.TimetableProviderClient _client;

    // логгер
    private readonly ILogger<GRPCTimetableProvider> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger">Логгер</param>
    /// <param name="serviceAddress">URI сервиса</param>
    public GRPCTimetableProvider(ILogger<GRPCTimetableProvider> logger, string serviceAddress)
    {
        _logger = logger;
        _serviceAddress = serviceAddress;
        
        // init gRPC client
        var c = GrpcChannel.ForAddress(_serviceAddress);
        _client = new TimetableProvider.TimetableProviderClient(c);
    }

    public TimetableResult GetTimetable(TimetableRequestArgs requestArgs)
    {
        try
        { 
            var timetable = _client.GetTimetable(new TimetableRequest
            {
                Group = requestArgs.Group ?? "",
                Teacher = requestArgs.Teacher ?? "",
                Building = requestArgs.Building ?? "",
                ClassRoom = requestArgs.ClassRoom ?? ""
            });

            // Обнуляем счетчик, если удалось запросить
            _reconnectCounter = 0;
            _logger.Log(LogLevel.Information, "Received timetable from service");

            var actualWeekType = (Domain.Timetable.WeekTypes)(int)timetable.ActualWeekType;

            // map proto lesson to domain lesson
            var lessons = timetable.Lessons.AsEnumerable().Select((lesson) =>
            {
                return new Domain.Timetable.Lesson
                {
                    Groups = lesson.Groups,
                    Building = lesson.Building,
                    ClassRoom = lesson.ClassRoom,
                    Teacher = lesson.Teacher,
                    WeekDay = (Domain.Timetable.WeekDays)(int)lesson.WeekDay,
                    WeekType = (Domain.Timetable.WeekTypes)(int)lesson.WeekType,
                    LessonType = (Domain.Timetable.LessonTypes)(int)lesson.LessonType,
                    LessonName = lesson.LessonName,
                    EndTime = lesson.EndTime,
                    OrderNumber = lesson.OrderNumber,
                    StartTime = lesson.StartTime,

                };
            });
            return new TimetableResult(actualWeekType, lessons);
        }
        // Если во время запроса произошла ошибка, то вылетит RpcException
        catch (RpcException e)
        {
            if (e.StatusCode is StatusCode.Unavailable or StatusCode.DeadlineExceeded)
            {   
                // if we not connected to the service, we trying to reconnect
                Reconnect();
                return GetTimetable(requestArgs);
            }
            // Если статус код не подразумевает ошибку соединения, то кидаем эксепшн выше по стеку
            throw;
        }
    }

    /// <summary>
    /// Метод, отвечающий за переподключение к сервису
    /// </summary>
    /// <exception cref="ServiceUnavailableException">Выбрасывается, если сервис не доступен</exception>
    private void Reconnect()
    {
        if (_reconnectCounter == 3)
        {
            throw new ServiceUnavailableException();
        }

        // trying to reconnect
        var channel = GrpcChannel.ForAddress(_serviceAddress);
        _client = new TimetableProvider.TimetableProviderClient(channel);
        ++_reconnectCounter;
    }
}
