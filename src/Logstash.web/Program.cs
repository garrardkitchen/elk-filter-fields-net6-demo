using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json").AddEnvironmentVariables();
builder.Host.UseSerilog( (ctx, lx) => 
    lx.MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.File(new JsonFormatter(), ctx.Configuration.GetValue<string>("logfile"))
    .WriteTo.Console(new JsonFormatter())
);

var app = builder.Build();

app.MapGetToTriggerLogEntryForMessage();
app.MapGetToTriggerLogEntryForApplicationData();
app.Services.GetService<Serilog.ILogger>()?.Information("Started");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

// References:
// - https://blog.datalust.co/using-serilog-in-net-6/

record MessageDto (Guid id);