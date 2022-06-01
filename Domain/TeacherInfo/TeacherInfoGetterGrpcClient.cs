using Grpc.Net.Client;
using Suai.Bot.TeacherInfo.Proto;

namespace suai_api.Domain.TeacherInfo;

public class TeacherInfoGetterGrpcClient : GrpcClientWithReconnect<TeacherInfoProvider.TeacherInfoProviderClient, TeacherInfoReply, TeacherInfoRequest>
{
    public TeacherInfoGetterGrpcClient(string serviceURI, uint maxRetryCount = 0) 
        : base(() => 
        {
            var channel = GrpcChannel.ForAddress(serviceURI);
            return new TeacherInfoProvider.TeacherInfoProviderClient(channel);
        }, maxRetryCount)
    {
    }

    protected override TeacherInfoReply MakeGrpcRequest(TeacherInfoRequest request)
    {
        return _client.GetTeacherInfo(request);
    }
}
