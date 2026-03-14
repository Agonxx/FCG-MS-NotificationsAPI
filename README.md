# NotificationsAPI

Microsserviço responsável pelo envio de notificações (simuladas via log) na plataforma FIAP Cloud Games.

## Responsabilidades

- Consumir o evento `UserCreatedEvent` e simular envio de e-mail de boas-vindas
- Consumir o evento `PaymentProcessedEvent` e simular envio de e-mail de confirmação de compra

## Fluxo de eventos

```
UsersAPI    → UserCreatedEvent     → NotificationsAPI (boas-vindas)
PaymentsAPI → PaymentProcessedEvent → NotificationsAPI (confirmação de compra)
```

## Variáveis de ambiente

| Variável | Descrição | Exemplo |
|---|---|---|
| `RabbitMQ__Host` | Host do broker RabbitMQ | `rabbitmq` |
| `RabbitMQ__VHost` | Virtual host do RabbitMQ | `/` |
| `RabbitMQ__Username` | Usuário do RabbitMQ | `guest` |
| `RabbitMQ__Password` | Senha do RabbitMQ | `guest` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente da aplicação | `Production` |

> **Nota:** Este serviço **não possui banco de dados** nem autenticação JWT. É um consumidor puro de eventos.

## Executar localmente

```bash
# A partir da raiz da solução
docker-compose up --build notificationsapi
```

Ou via Docker isolado:

```bash
docker build -f NotificationsAPI/NotificationsAPI.Api/Dockerfile -t notificationsapi:latest .
docker run -p 5004:8080 notificationsapi:latest
```

## Exemplo de saída nos logs

```
info: EMAIL | Boas-vindas | Para: joao@email.com | Nome: João Silva
info: EMAIL | Confirmação de compra | UserId: 2 | JogoId: 5 | Jogo: Elden Ring | Preço: R$199,90
```

## Observações

- As notificações são **simuladas**: nenhum e-mail real é enviado. Toda a saída é via `ILogger`.
- Por ser um serviço sem estado e sem banco, é o mais leve da solução.
- Apenas depende do RabbitMQ estar saudável para iniciar.

## Tecnologias

- .NET 9
- MassTransit 8 + RabbitMQ
