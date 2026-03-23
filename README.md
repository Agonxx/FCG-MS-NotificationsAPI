# FCG-MS-NotificationsAPI

Microsserviço responsável pelo envio de notificações (simuladas via log) na plataforma **FIAP Cloud Games**.

## Responsabilidades

- Consumir `UserCreatedEvent` → simular e-mail de boas-vindas
- Consumir `PaymentProcessedEvent` → simular e-mail de confirmação de compra

## Fluxo de eventos

```
UsersAPI    → [UserCreatedEvent]       → NotificationsAPI (boas-vindas)
PaymentsAPI → [PaymentProcessedEvent]  → NotificationsAPI (confirmação de compra)
```

---

## Pré-requisitos

| Ferramenta | Versão mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/9.0) | 9.0 |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | 24+ |
| RabbitMQ | 3.13+ (via Docker) |

> **Sem banco de dados** — este serviço é stateless, apenas consome eventos e loga.

---

## Variáveis de ambiente

| Variável | Descrição | Valor padrão (dev) |
|---|---|---|
| `RabbitMQ__Host` | Host do RabbitMQ | `localhost` |
| `RabbitMQ__VHost` | Virtual host | `/` |
| `RabbitMQ__Username` | Usuário | `guest` |
| `RabbitMQ__Password` | Senha | `guest` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente | `Development` |

---

## Como executar

### 1. Subir RabbitMQ

```bash
docker run -d --name rabbitmq \
  -p 5672:5672 -p 15672:15672 \
  rabbitmq:3-management
```

Painel de gestão: `http://localhost:15672` (usuário: `guest` / senha: `guest`)

### 2. Executar via .NET CLI (desenvolvimento)

```bash
cd NotificationsAPI.Api
dotnet run
```

### 3. Executar via Docker (imagem isolada)

```bash
# Build a partir da raiz do repositório
docker build -t fcg-notificationsapi:latest .

# Run
docker run -d --name notificationsapi -p 5004:8080 \
  -e "RabbitMQ__Host=host.docker.internal" \
  -e "RabbitMQ__Username=guest" \
  -e "RabbitMQ__Password=guest" \
  fcg-notificationsapi:latest
```

```bash
# Ver logs em tempo real
docker logs -f notificationsapi
```

### 4. Executar via Kubernetes

```bash
kubectl apply -f k8s/
kubectl get pods -n fiapcloudgames
kubectl logs -l app=notificationsapi -n fiapcloudgames
```

---

## Verificar funcionamento

Não há endpoints de negócio. O serviço processa eventos e emite logs:

```
info: EMAIL | Boas-vindas       | Para: joao@email.com | Nome: João Silva
info: EMAIL | Confirmação       | UserId: 2 | JogoId: 5 | Jogo: Elden Ring | Preço: R$199,90
```

Para forçar um teste end-to-end:
1. Cadastre um usuário no **UsersAPI** → `NotificationsAPI` logará o e-mail de boas-vindas
2. Compre um jogo no **CatalogAPI** → `PaymentsAPI` processará e `NotificationsAPI` logará a confirmação

---

## Estrutura do projeto

```
FCG-MS-NotificationsAPI/
├── Dockerfile
├── k8s/
├── NotificationsAPI.Api/         # Program.cs, Extensions, Consumers
└── NotificationsAPI.Application/ # Handlers de notificação
```

---

## Observações

- Nenhum e-mail real é enviado — toda saída é via `ILogger`.
- Serviço stateless: sem banco de dados, sem JWT, sem estado.
- O mais leve da solução — depende apenas do RabbitMQ estar saudável.

---

## Tecnologias

- .NET 9 / ASP.NET Core 9
- MassTransit 8.3.6 + RabbitMQ
