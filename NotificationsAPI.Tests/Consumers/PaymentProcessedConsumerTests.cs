using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationsAPI.Application.Consumers;
using NotificationsAPI.Domain.Interfaces;
using Shared.Contracts.Events;
using Xunit;

namespace NotificationsAPI.Tests.Consumers;

public class PaymentProcessedConsumerTests
{
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ILogger<PaymentProcessedConsumer>> _loggerMock;
    private readonly PaymentProcessedConsumer _consumer;

    public PaymentProcessedConsumerTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _loggerMock = new Mock<ILogger<PaymentProcessedConsumer>>();
        _consumer = new PaymentProcessedConsumer(_notificationServiceMock.Object, _loggerMock.Object);
    }

    private PaymentProcessedEvent CriarEvento(string status) =>
        new(Guid.NewGuid(), 1, 5, "Elden Ring", 199.90m, status, DateTime.UtcNow);

    [Fact]
    public async Task Consume_QuandoApproved_DeveEnviarConfirmacao()
    {
        var ev = CriarEvento("Approved");
        var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
        contextMock.Setup(c => c.Message).Returns(ev);

        await _consumer.Consume(contextMock.Object);

        _notificationServiceMock.Verify(
            s => s.EnviarConfirmacaoCompra(ev.UserId, ev.JogoId, ev.Titulo, ev.Preco),
            Times.Once);
    }

    [Fact]
    public async Task Consume_QuandoNaoApproved_NaoDeveEnviarConfirmacao()
    {
        var ev = CriarEvento("Rejected");
        var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
        contextMock.Setup(c => c.Message).Returns(ev);

        await _consumer.Consume(contextMock.Object);

        _notificationServiceMock.Verify(
            s => s.EnviarConfirmacaoCompra(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<decimal>()),
            Times.Never);
    }

    [Fact]
    public async Task Consume_NaoDeveLancarExcecao()
    {
        var ev = CriarEvento("Approved");
        var contextMock = new Mock<ConsumeContext<PaymentProcessedEvent>>();
        contextMock.Setup(c => c.Message).Returns(ev);

        var ex = await Record.ExceptionAsync(() => _consumer.Consume(contextMock.Object));
        Assert.Null(ex);
    }
}
