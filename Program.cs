using Microsoft.AspNetCore.Hosting.Server.Features;
using suai_api_schedule.Models;



var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseSetting("https_port", "443");

builder.Configuration.AddCommandLine(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITimeTableProvider>((x) =>
{
    //var serviceUrl = builder.Configuration.GetValue<string>("schedule_service_addr");
    var serviceUrl = "http://127.0.0.1:2288/";
    return new GRPCTimeTableProvider(x.GetRequiredService<ILogger<GRPCTimeTableProvider>>(), serviceUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Map("/", () =>
{
    return "Hello World";
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
