namespace suai_api.Domain.TeacherInfo;

public record class TeacherInfoDto(
    string Name, 
    string Institute,
    string Department,
    uint Phone,
    string Email,
    string AcademicDegree,
    string Position
    );
