using Microsoft.AspNetCore.Hosting.Server.Features;
using suai_api_schedule.Models;
using suai_api_schedule.Models.TimeTable;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSetting("https_port", "443");

builder.Configuration.AddCommandLine(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSingleton<ITimeTableProvider>((x) =>
{
    var serviceUrl = builder.Configuration.GetValue<string>("schedule_service_addr");
    return new GRPCTimeTableProvider(x.GetRequiredService<ILogger<GRPCTimeTableProvider>>(), serviceUrl);
});

var app = builder.Build();

app.Map("/", () =>
{
    return $"This is main page\nUse {app.Urls.First()}/api.[service].[method]/ to use api\n";
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
