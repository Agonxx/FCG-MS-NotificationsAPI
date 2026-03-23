using Microsoft.Extensions.Logging;
using Moq;
using NotificationsAPI.Application.Services;
using Xunit;

namespace NotificationsAPI.Tests.Services;

public class NotificationServiceTests
{
    private readonly Mock<ILogger<NotificationService>> _loggerMock;
    private readonly NotificationService _service;

    public NotificationServiceTests()
    {
        _loggerMock = new Mock<ILogger<NotificationService>>();
        _service = new NotificationService(_loggerMock.Object);
    }

    [Fact]
    public void EnviarBoasVindas_DeveLogarMensagem()
    {
        _service.EnviarBoasVindas("João Silva", "joao@email.com");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Boas-vindas")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void EnviarBoasVindas_NaoDeveLancarExcecao()
    {
        var ex = Record.Exception(() => _service.EnviarBoasVindas("Maria", "maria@email.com"));
        Assert.Null(ex);
    }

    [Fact]
    public void EnviarConfirmacaoCompra_DeveLogarMensagem()
    {
        _service.EnviarConfirmacaoCompra(1, 5, "Elden Ring", 199.90m);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Confirmação de compra")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void EnviarConfirmacaoCompra_NaoDeveLancarExcecao()
    {
        var ex = Record.Exception(() => _service.EnviarConfirmacaoCompra(2, 3, "GTA VI", 299.90m));
        Assert.Null(ex);
    }
}
