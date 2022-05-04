using Grpc.Core;
using Grpc.Net.Client;
using Suai.Bot.Timetable.Proto;
using System.Runtime.ExceptionServices;

namespace suai_api.Domain.TimeTable;

/// <summary>
/// Класс, предназначенный для получения расписания по Grpc
/// </summary>
public class TimetableGetterGrpcClient : GrpcClient<TimetableProvider.TimetableProviderClient, TimetableReply, TimetableRequest>
{
    private readonly uint _maxRetryCount;
    private uint _retryCount = 0;

    public TimetableGetterGrpcClient(string serviceURI, uint maxRetryCount = 0) : base(() => 
    {
        var channel = GrpcChannel.ForAddress(serviceURI);
        return new TimetableProvider.TimetableProviderClient(channel);
    })
    {
        _maxRetryCount = maxRetryCount;
    }

    protected override TimetableReply HandleRequestFailed(Exception ex, TimetableRequest request)
    {
        if (ex is RpcException rpcEx) 
        {
            if (rpcEx.StatusCode is not StatusCode.Unavailable and StatusCode.DeadlineExceeded)
                ExceptionDispatchInfo.Capture(ex).Throw(); // (https://stackoverflow.com/questions/57383/how-to-rethrow-innerexception-without-losing-stack-trace-in-c)

            if (_retryCount == _maxRetryCount)
                ExceptionDispatchInfo.Capture(ex).Throw();

            _retryCount++;
            _client = _clientFactory();
            return GetData(request);
        }

        ExceptionDispatchInfo.Capture(ex).Throw();

        // Компилятор ругается из-за ExceptionDispatchInfo.Capture(ex).Throw() (т.к. думает что это дефолт метод),
        // так что кидаем полученный эксепшн для его спокойствия. На самом деле до этого участка кода мы никогда не дойдем
        throw ex;
    }

    protected override void HandleRequestSucceed(TimetableReply response)
    {
        // Сбрасываем счетчик, если запрос прошел успешно
        _retryCount = 0;
    }

    protected override TimetableReply MakeGrpcRequest(TimetableRequest request)
    {
        return _client.GetTimetable(request);
    }
}