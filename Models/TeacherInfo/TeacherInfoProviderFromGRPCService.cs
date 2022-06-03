using suai_api.Domain.TeacherInfo;
using Suai.Bot.TeacherInfo.Proto;

namespace suai_api.Models.TeacherInfo
{
    public class TeacherInfoProviderFromGRPCService : ITeacherInfoProvider
    {
        private readonly TeacherInfoGetterGrpcClient _client;
        private readonly ILogger<TeacherInfoProviderFromGRPCService> _logger;

        public TeacherInfoProviderFromGRPCService(ILogger<TeacherInfoProviderFromGRPCService> logger, string serviceURI)
        {
            _logger = logger;
            _client = new TeacherInfoGetterGrpcClient(serviceURI,maxRetryCount: 3);
        }
        public TeacherInfoReply GetTeacherInfo(string surname)
        {
            return _client.GetData(new TeacherInfoRequest { LastName = surname });
        }
    }
}
