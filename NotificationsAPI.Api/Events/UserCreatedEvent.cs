namespace Shared.Contracts.Events;

public record UserCreatedEvent(
    int UserId,
    string Nome,
    string Email,
    DateTime CriadoEm
);
