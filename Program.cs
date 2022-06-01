using suai_api.Models.TeacherInfo;
using suai_api.Models.Timetable;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddSingleton<ITimetableProvider>((x) =>
{
    var serviceUrl = builder.Configuration.GetValue<string>("TIMETABLE_SERVICE_ADDR");
    return new TimetableProviderFromGRPCService(x.GetRequiredService<ILogger<TimetableProviderFromGRPCService>>(), serviceUrl);
});

builder.Services.AddSingleton<ITeacherInfoProvider>((x) =>
{
    var serviceUrl = builder.Configuration.GetValue<string>("TEACHER_SERVICE_ADDR");
    return new TeacherInfoProviderFromGRPCService(x.GetRequiredService<ILogger<TeacherInfoProviderFromGRPCService>>(), serviceUrl);
});

var app = builder.Build();

app.Map("/", () =>
{
    return $"This is main page\nAdd \"/[university_name].[service].[method]/\" to address string to use api\n";
});


app.MapControllers();

app.Run();
