# Favly API

> API para gerenciamento de tarefas e despesas de grupos e indivíduos.

---

## 📖 Sobre o Projeto

O **Favly** é uma API RESTful desenvolvida para facilitar a organização de **tarefas** e **despesas** dentro de grupos (famílias, repúblicas, times) e de forma individual.

Com o Favly é possível:

- Criar e gerenciar **grupos** com membros e convites
- Definir **tarefas recorrentes** do grupo ou individuais (diária, semanal, mensal)
- Controlar **despesas** do grupo ou pessoais com recorrência e vencimento
- Atribuir tarefas a membros específicos
- Gerenciar **subtarefas** vinculadas a tarefas pai

### Exemplo de uso real

> *Daylane pertence ao grupo "Triângulo". Dentro do grupo, ela tem a tarefa semanal de lavar a roupa e a despesa mensal de condomínio todo dia 10. Individualmente, ela tem a tarefa de estudar chinês toda segunda, quarta e sexta, e a despesa de internet todo dia 20.*

---

## 🏛️ Arquitetura

O projeto segue os princípios de **Clean Architecture**, **DDD (Domain-Driven Design)**, **CQRS** e **KISS**, organizado em 4 camadas:

```
Favly/
├── Favly.Domain/           # Entidades, Value Objects, Events, Interfaces
├── Favly.Application/      # Commands, Queries, Handlers, Validators, DTOs
├── Favly.Infrastructure/   # Repositórios, EF Core, Segurança, Migrations
└── Favly.api/              # Controllers, Middlewares, Extensions, Program.cs
```

### Fluxo de uma requisição

```
HTTP Request
    └── Controller
            └── IMessageBus (Wolverine)
                    └── Handler (Application)
                            └── Repository (Infrastructure)
                                    └── Entity / Domain Events (Domain)
```

### Princípios aplicados

| Princípio | Como é aplicado |
|---|---|
| **DDD** | Entidades ricas com comportamentos, Value Objects com validação, Domain Events em cada operação |
| **Clean Arch** | Dependências apontam sempre para dentro — Domain não conhece Infrastructure |
| **CQRS** | Commands e Queries separados, cada operação em seu próprio diretório com Handler e Validator |
| **KISS** | Handlers estáticos, DTOs com `FromEntity()`, sem AutoMapper, sem abstrações desnecessárias |

---

## 🧱 Domínio

### Entidades

| Entidade | Descrição |
|---|---|
| `Usuario` | Usuário da plataforma com ativação por código |
| `Grupo` | Grupo de pessoas (família, república, time) |
| `Membro` | Vínculo entre Usuário e Grupo com papel (Admin, Membro) |
| `Convite` | Convite por e-mail para entrar em um grupo, com expiração |
| `Tarefa` | Tarefa individual ou do grupo com recorrência e subtarefas |
| `Pagamento` | Despesa individual ou do grupo com recorrência e vencimento |

### Value Objects

| Value Object | Descrição |
|---|---|
| `EmailUsuario` | E-mail validado com regex e restrições de domínio |
| `RecorrenciaTarefa` | Frequência, intervalo e dias da semana de uma tarefa |
| `RecorrenciaPagamento` | Dia de vencimento e frequência de uma despesa |
| `DinheiroPagamento` | Valor monetário com moeda (padrão BRL) |

### Domain Events

Cada operação de domínio emite um evento:

- `UsuarioCriadoEvent`, `UsuarioAtivadoEvent`, `UsuarioAtualizadoEvent`, `UsuarioDesativadoEvent`
- `TarefaConcluidaEvent`
- `PagamentoRealizadoEvent`

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| **.NET** | 10.0 | Framework principal |
| **PostgreSQL** | 17 | Banco de dados relacional |
| **Entity Framework Core** | 10.x | ORM e migrations |
| **Wolverine** | 5.x | Mensageria, CQRS e gerenciamento de transações |
| **FluentValidation** | latest | Validação dos Commands |
| **BCrypt.Net** | latest | Hash de senhas |
| **Scrutor** | 7.x | Auto-registro de dependências por convenção |
| **JWT Bearer** | 10.x | Autenticação via token |
| **Swagger / Swashbuckle** | 7.x | Documentação interativa da API |
| **Docker + Docker Compose** | latest | Containerização do banco e da aplicação |

---

## 📁 Estrutura de pastas

```
Favly.Application/
└── Usuarios/
    ├── Commands/
    │   ├── CriarUsuario/
    │   │   ├── CriarUsuarioCommand.cs
    │   │   ├── CriarUsuarioHandler.cs
    │   │   └── CriarUsuarioValidator.cs
    │   ├── AtivarUsuario/
    │   ├── AtualizarUsuario/
    │   └── DesativarUsuario/
    ├── Queries/
    │   ├── ObterUsuarioPorId/
    │   └── ObterUsuarioPorEmail/
    └── DTOs/
        └── UsuarioResponse.cs

Favly.Domain/
├── Entities/
├── Events/
├── Interfaces/
├── ValueObjects/
└── Common/
    ├── Base/        # Entity, AggregateRoot, ValueObject, IDomainEvent
    ├── Enums/
    ├── Exceptions/  # DomainException, NotFoundException
    └── Validations/ # Guard

Favly.Infrastructure/
├── Data/
│   ├── FavlyDbContext.cs
│   ├── Configurations/  # IEntityTypeConfiguration por entidade
│   └── Migrations/
├── Repositories/
├── Security/        # BcryptPasswordHasher, TokenService
└── Extensions/      # InfrastructureExtensions (Scrutor scan)
```

---

## 🚀 Como rodar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

### 1. Clone o repositório

```bash
git clone https://github.com/daylane/Favly.git
cd Favly
```

### 2. Configure os segredos locais

```bash
dotnet user-secrets init --project Favly.api
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_JWT_AQUI" --project Favly.api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=favly_db;Username=postgres;Password=SUA_SENHA;SSL Mode=Disable" --project Favly.api
```

### 3. Suba o banco com Docker

```bash
docker-compose up favly-db -d
```

### 4. Rode a aplicação

```bash
dotnet run --project Favly.api
```

A API estará disponível em `http://localhost:8082` e o Swagger em `http://localhost:8082/index.html`.

### Ou rode tudo com Docker Compose

```bash
docker-compose up --build
```

---

## 🔐 Autenticação

A API usa **JWT Bearer Token**. Para acessar endpoints protegidos:

---


## Projeto desenvolvido para fins de aprendizado e uso pessoal.

