using Grpc.Core;
using System.Runtime.ExceptionServices;

namespace suai_api
{
    public abstract class GrpcClientWithReconnect<TClient, TResponse, TRequest> : GrpcClient<TClient, TResponse, TRequest>
        where TClient : Grpc.Core.ClientBase
        where TResponse : Google.Protobuf.IMessage<TResponse>
        where TRequest : Google.Protobuf.IMessage<TRequest>

    {
        private readonly uint _maxRetryCount;
        private uint _retryCount;

        protected GrpcClientWithReconnect(Func<TClient> clientFactory, uint maxRetryCount = 0) : base(clientFactory)
        {
            _maxRetryCount = maxRetryCount;
        }

        protected override TResponse HandleRequestFailed(Exception ex, TRequest request)
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

        protected override void HandleRequestSucceed(TResponse response)
        {
            // Сбрасываем счетчик, если запрос прошел успешно
            _retryCount = 0;
        }
    }
}
