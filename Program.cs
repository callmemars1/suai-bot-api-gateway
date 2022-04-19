using suai_api_schedule.Models;
using suai_api_schedule;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IScheduleProvider>((x) => 
{
    var serviceUrl = builder.Configuration.GetValue<string>("ScheduleServiceUrl");
    return new GRPCScheduleProvider(x.GetRequiredService<ILogger<GRPCScheduleProvider>>(), serviceUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}   

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
