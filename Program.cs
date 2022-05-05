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
    return $"This is main page\nAdd \"/api.[service].[method]/\" to address string to use api\n";
});


Console.WriteLine("Some text to check");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
