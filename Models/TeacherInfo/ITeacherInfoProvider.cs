using suai_api.Domain.TeacherInfo;

namespace suai_api.Models.TeacherInfo;

public interface ITeacherInfoProvider
{
    TeacherInfoDto GetTeacherInfo(string name);
}
