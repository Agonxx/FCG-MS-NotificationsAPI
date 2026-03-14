using Microsoft.Extensions.Logging;
using NotificationsAPI.Domain.Interfaces;

namespace NotificationsAPI.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public void EnviarBoasVindas(string nome, string email)
        {
            _logger.LogInformation(
                "EMAIL | Boas-vindas | Para: {Email} | Nome: {Nome}",
                email, nome);
        }

        public void EnviarConfirmacaoCompra(int userId, int jogoId, string titulo, decimal preco)
        {
            _logger.LogInformation(
                "EMAIL | Confirmação de compra | UserId: {UserId} | JogoId: {JogoId} | Jogo: {Titulo} | Preço: {Preco:C}",
                userId, jogoId, titulo, preco);
        }
    }
}
