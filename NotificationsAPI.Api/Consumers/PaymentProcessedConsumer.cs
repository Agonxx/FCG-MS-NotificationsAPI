using MassTransit;
using NotificationsAPI.Domain.Interfaces;
using Shared.Contracts.Events;

namespace NotificationsAPI.Api.Consumers
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessedEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<PaymentProcessedConsumer> _logger;

        public PaymentProcessedConsumer(INotificationService notificationService, ILogger<PaymentProcessedConsumer> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<PaymentProcessedEvent> context)
        {
            var ev = context.Message;

            _logger.LogInformation(
                "PaymentProcessedEvent recebido: OrderId={OrderId} | UserId={UserId} | Status={Status}",
                ev.OrderId, ev.UserId, ev.Status);

            if (ev.Status != "Approved")
                return Task.CompletedTask;

            _notificationService.EnviarConfirmacaoCompra(ev.UserId, ev.JogoId, ev.Titulo, ev.Preco);

            return Task.CompletedTask;
        }
    }
}
