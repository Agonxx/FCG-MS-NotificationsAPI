using NotificationsAPI.Api.Extensions;
using NotificationsAPI.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices()
                .AddApiDocumentation()
                .AddApiCors()
                .AddMessaging(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>()
    .UseMiddleware<ExceptionMiddleware>()
    .UseMiddleware<RequestLoggingMiddleware>()
    .UseCors("DefaultCors");

app.MapControllers();

app.Run();
