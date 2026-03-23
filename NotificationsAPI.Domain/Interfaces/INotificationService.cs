namespace NotificationsAPI.Domain.Interfaces;

public interface INotificationService
{
    void EnviarBoasVindas(string nome, string email);
    void EnviarConfirmacaoCompra(int userId, int jogoId, string titulo, decimal preco);
}
