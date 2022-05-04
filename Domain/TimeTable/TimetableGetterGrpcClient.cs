using Grpc.Net.Client;
using Suai.Bot.Timetable.Proto;

namespace suai_api.Domain.TimeTable;

/// <summary>
/// Класс, предназначенный для получения расписания по Grpc
/// </summary>
public class TimetableGetterGrpcClient : GrpcClientWithReconnect<TimetableProvider.TimetableProviderClient, TimetableReply, TimetableRequest>
{
    public TimetableGetterGrpcClient(string serviceURI, uint maxRetryCount = 0) : base(() => 
    {
        var channel = GrpcChannel.ForAddress(serviceURI);
        return new TimetableProvider.TimetableProviderClient(channel);
    }, maxRetryCount)
    {
    }

    protected override TimetableReply MakeGrpcRequest(TimetableRequest request)
    {
        return _client.GetTimetable(request);
    }
}