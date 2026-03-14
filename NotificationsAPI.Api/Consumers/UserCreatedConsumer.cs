using MassTransit;
using NotificationsAPI.Domain.Interfaces;
using Shared.Contracts.Events;

namespace NotificationsAPI.Api.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<UserCreatedConsumer> _logger;

        public UserCreatedConsumer(INotificationService notificationService, ILogger<UserCreatedConsumer> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var ev = context.Message;

            _logger.LogInformation(
                "UserCreatedEvent recebido: UserId={UserId} | Email={Email}",
                ev.UserId, ev.Email);

            _notificationService.EnviarBoasVindas(ev.Nome, ev.Email);

            return Task.CompletedTask;
        }
    }
}
