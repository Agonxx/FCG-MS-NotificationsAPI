using MassTransit;
using Microsoft.OpenApi.Models;
using NotificationsAPI.Api.Consumers;
using NotificationsAPI.Application.Services;
using NotificationsAPI.Domain.Interfaces;

namespace NotificationsAPI.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        public static IServiceCollection AddApiCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCors", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NotificationsAPI",
                    Version = "v1",
                    Description = "Serviço de notificações - FIAP Cloud Games"
                });
            });

            return services;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedConsumer>();
                x.AddConsumer<PaymentProcessedConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VHost"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });

            return services;
        }
    }
}
