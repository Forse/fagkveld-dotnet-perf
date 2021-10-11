using DotNetPerf.Application;
using DotNetPerf.Domain;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(logging => logging.SingleLine = true);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DotNetPerf.Api", Version = "v1" });
});

builder.Services.AddOpenTelemetryTracing(
    builder => builder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("DotNetPerf"))
        .AddSource(Diagnostics.ActivitySource.Name)
        .AddAspNetCoreInstrumentation()
        .AddJaegerExporter()
);

builder.Services.AddApplication();

#if RUN_TEST_CALCULATIONS
builder.Services.AddHostedService<TestCalculationsService>();
#endif

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetPerf.Api v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
