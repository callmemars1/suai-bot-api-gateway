using suai_api.Models.TeacherInfo;
using suai_api.Models.Timetable;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSetting("https_port", "443");

builder.Configuration.AddCommandLine(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<ITimetableProvider>((x) =>
{
    var serviceUrl = builder.Configuration.GetValue<string>("schedule_service_addr");
    return new TimetableProviderFromGRPCService(x.GetRequiredService<ILogger<TimetableProviderFromGRPCService>>(), serviceUrl);
});

builder.Services.AddSingleton<ITeacherInfoProvider>((x) =>
{
    var serviceUrl = builder.Configuration.GetValue<string>("schedule_service_addr");
    return new TeacherInfoProviderFromGRPCService(x.GetRequiredService<ILogger<TeacherInfoProviderFromGRPCService>>(), serviceUrl);
});

var app = builder.Build();

app.Map("/", () =>
{
    return $"This is main page\nAdd \"/[university_name].[service].[method]/\" to address string to use api\n";
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
