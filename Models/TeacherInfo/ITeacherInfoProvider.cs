using Suai.Bot.TeacherInfo.Proto;

namespace suai_api.Models.TeacherInfo;

public interface ITeacherInfoProvider
{
    TeacherInfoReply GetTeacherInfo(string surname);
}
