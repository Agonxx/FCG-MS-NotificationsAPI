using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationsAPI.Application.Consumers;
using NotificationsAPI.Domain.Interfaces;
using Shared.Contracts.Events;
using Xunit;

namespace NotificationsAPI.Tests.Consumers;

public class UserCreatedConsumerTests
{
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ILogger<UserCreatedConsumer>> _loggerMock;
    private readonly UserCreatedConsumer _consumer;

    public UserCreatedConsumerTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _loggerMock = new Mock<ILogger<UserCreatedConsumer>>();
        _consumer = new UserCreatedConsumer(_notificationServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Consume_DeveCharmarEnviarBoasVindas()
    {
        var ev = new UserCreatedEvent(1, "João Silva", "joao@email.com", DateTime.UtcNow);
        var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
        contextMock.Setup(c => c.Message).Returns(ev);

        await _consumer.Consume(contextMock.Object);

        _notificationServiceMock.Verify(
            s => s.EnviarBoasVindas("João Silva", "joao@email.com"),
            Times.Once);
    }

    [Fact]
    public async Task Consume_NaoDeveLancarExcecao()
    {
        var ev = new UserCreatedEvent(2, "Maria", "maria@email.com", DateTime.UtcNow);
        var contextMock = new Mock<ConsumeContext<UserCreatedEvent>>();
        contextMock.Setup(c => c.Message).Returns(ev);

        var ex = await Record.ExceptionAsync(() => _consumer.Consume(contextMock.Object));
        Assert.Null(ex);
    }
}
