using Grpc.Core;
using Grpc.Net.Client;
using Suai.Bot.Timetable.Proto;
using suai_api.Domain.Timetable.Exceptions;

namespace suai_api.Domain.TimeTable;

public class TimetableServiceGrpcClient
{
    private readonly string _serviceURI;

    private TimetableProvider.TimetableProviderClient _client;

    private uint _maxReconnectCounter = 0;

    private uint _reconnectCounter = 0;

    public TimetableServiceGrpcClient(string serviceURI)
    {
        _serviceURI = serviceURI;

        // init gRPC client
        var c = GrpcChannel.ForAddress(_serviceURI);
        _client = new TimetableProvider.TimetableProviderClient(c);
    }

    public TimetableReply GetTimetable(TimetableRequest request, uint maxReconnects = 0)
    {
        _maxReconnectCounter = maxReconnects;
        try
        {
            return _client.GetTimetable(request);
        }
        catch (RpcException ex) when (ex.StatusCode is StatusCode.Unavailable or StatusCode.DeadlineExceeded)
        {
            Reconnect();
            return GetTimetable(request);
        }
    }
    
    /// <summary>
    /// Метод, отвечающий за переподключение к сервису
    /// </summary>
    /// <exception cref="ServiceUnavailableException">Выбрасывается, если сервис не доступен</exception>
    private void Reconnect()
    {
        if (_reconnectCounter == _maxReconnectCounter)
            throw new ServiceUnavailableException();

        // Попытка переподключения
        var channel = GrpcChannel.ForAddress(_serviceURI);
        _client = new TimetableProvider.TimetableProviderClient(channel);
        ++_reconnectCounter;
    }
}
