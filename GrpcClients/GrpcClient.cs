namespace suai_api;

public abstract class GrpcClient<TClient, TResponse, TRequest>
    where TClient : Grpc.Core.ClientBase
    where TResponse : Google.Protobuf.IMessage<TResponse>
    where TRequest : Google.Protobuf.IMessage<TRequest>
{
    protected readonly Func<TClient> _clientFactory;
    protected TClient _client;


    /// <param name="clientFactory">Фабрика, создающая объект нужного Grpc клиента</param>
    public GrpcClient(Func<TClient> clientFactory)
    {
        _clientFactory = clientFactory;
        _client = _clientFactory.Invoke();
    }

    public TResponse GetData(TRequest request) 
    {
        try
        {
            var result = MakeGrpcRequest(request);
            HandleRequestSucceed(result);
            return result;
        }
        catch (Exception ex)
        {
            return HandleRequestFailed(ex, request);
        }
    }

    /// <summary>
    /// Метод вызывается, если в запросе что-то пошло не так
    /// </summary>
    /// <param name="ex">Ошибка, по которой запрос не выполнился</param>
    /// <param name="request">Данные, переданные в запрос</param>
    /// <returns>Результат, который необходимо вернуть в случае ошибки.</returns>
    protected abstract TResponse HandleRequestFailed(Exception ex, TRequest request);

    /// <summary>
    /// Метод вызывается, если запрос удался
    /// </summary>
    /// <param name="response">Результат запроса</param>
    protected virtual void HandleRequestSucceed(TResponse response) 
    {
        // По умолчанию мы ничего не делаем,
        // но данный метод может быть нужен в случае реализации реконнектов и др.
    }

    /// <summary>
    /// Метод осуществляет запрос данных. 
    /// Т.к. базовый класс не знает какие методы есть
    /// у конкретного клиента, выполнение запроса перекладывается на дочерние классы.
    /// </summary>
    protected abstract TResponse MakeGrpcRequest(TRequest request);
}
