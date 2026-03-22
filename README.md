# 🏠 Favly API

> API RESTful para gerenciamento de tarefas e despesas de grupos e indivíduos.

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1?style=flat-square&logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-compose-2496ED?style=flat-square&logo=docker)
![CI](https://img.shields.io/badge/CI-GitHub_Actions-2088FF?style=flat-square&logo=githubactions)

---

## 📖 Sobre o Projeto

O **Favly** é uma API desenvolvida para facilitar a organização de **tarefas** e **despesas** dentro de grupos (famílias, repúblicas, times) e de forma individual.

### Exemplo de uso real

> *Daylane pertence ao grupo "Triângulo". Dentro do grupo, ela tem a tarefa semanal de lavar a roupa e a despesa mensal de condomínio todo dia 10. Individualmente, ela tem a tarefa de estudar chinês toda segunda, quarta e sexta, e a despesa de internet todo dia 20.*

Com o Favly é possível:

- Criar e gerenciar **grupos** com membros e convites por e-mail
- Definir **tarefas recorrentes** do grupo ou individuais (diária, semanal, mensal)
- Controlar **despesas** do grupo ou pessoais com recorrência e vencimento automático
- Atribuir tarefas a membros específicos do grupo
- Gerenciar **subtarefas** vinculadas a tarefas pai

---

## 🛠️ Tecnologias

### Backend
| Tecnologia | Versão | Função |
|---|---|---|
| **.NET** | 10.0 | Framework principal |
| **C#** | 13 | Linguagem de programação |
| **ASP.NET Core** | 10.0 | Framework web e API REST |

### Banco de Dados
| Tecnologia | Versão | Função |
|---|---|---|
| **PostgreSQL** | 17 | Banco de dados relacional |
| **Entity Framework Core** | 10.x | ORM, migrations e mapeamento |
| **Npgsql** | 10.x | Driver PostgreSQL para .NET |

### Mensageria e CQRS
| Tecnologia | Versão | Função |
|---|---|---|
| **Wolverine** | 5.x | Mensageria, CQRS e pipeline de handlers |
| **WolverineFx.EntityFrameworkCore** | 5.x | Integração Wolverine + EF Core |
| **WolverineFx.Postgresql** | 5.x | Persistência de mensagens no PostgreSQL |

### Segurança
| Tecnologia | Versão | Função |
|---|---|---|
| **JWT Bearer** | 10.x | Autenticação via token |
| **BCrypt.Net-Next** | latest | Hash seguro de senhas (workFactor 12) |

### Qualidade e Validação
| Tecnologia | Versão | Função |
|---|---|---|
| **FluentValidation** | latest | Validação de Commands e Queries |
| **Scrutor** | 7.x | Auto-registro de dependências por convenção |

### Documentação
| Tecnologia | Versão | Função |
|---|---|---|
| **Swashbuckle** | 7.x | Swagger UI com suporte a JWT |

### Testes
| Tecnologia | Versão | Função |
|---|---|---|
| **xUnit** | latest | Framework de testes |
| **FluentAssertions** | latest | Assertions expressivas |
| **NSubstitute** | latest | Mocks e stubs |
| **Bogus** | latest | Geração de dados falsos realistas |
| **Testcontainers.PostgreSql** | latest | PostgreSQL real em Docker para testes de integração |
| **Microsoft.AspNetCore.Mvc.Testing** | latest | Testes de integração da API |

### Infraestrutura
| Tecnologia | Versão | Função |
|---|---|---|
| **Docker** | latest | Containerização |
| **Docker Compose** | latest | Orquestração de containers |
| **GitHub Actions** | - | CI/CD — testes automáticos nos PRs |

---

## 🏛️ Arquitetura

O projeto segue os princípios de **Clean Architecture**, **DDD**, **CQRS** e **KISS**, organizado em 4 camadas com dependências sempre apontando para dentro:

```
Favly/
├── Favly.Domain/           # Entidades, Value Objects, Events, Interfaces
├── Favly.Application/      # Commands, Queries, Handlers, Validators, DTOs
├── Favly.Infrastructure/   # Repositórios, EF Core, Segurança, Migrations
├── Favly.api/              # Controllers, Middlewares, Extensions, Program.cs
└── Tests/
    ├── Favly.Tests.Unit/         # Testes unitários de Domain e Application
    └── Favly.Tests.Integration/  # Testes de integração com banco real
```

### Fluxo de uma requisição

```
HTTP Request
    └── Controller
            └── IMessageBus (Wolverine)
                    └── Handler (Application)
                            └── Repository (Infrastructure)
                                    └── Entity / Domain Events (Domain)
                                            └── UnitOfWork → SaveChanges
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
| `Grupo` | Grupo de pessoas (família, república, time) — Aggregate Root |
| `Membro` | Vínculo entre Usuário e Grupo com papel (Admin, Membro) |
| `Convite` | Convite por e-mail para entrar em um grupo, com expiração de 7 dias |
| `Tarefa` | Tarefa individual ou do grupo com recorrência e subtarefas |
| `Pagamento` | Despesa individual ou do grupo com recorrência e vencimento |

### Value Objects

| Value Object | Descrição |
|---|---|
| `EmailUsuario` | E-mail validado com regex, normalizado para lowercase |
| `RecorrenciaTarefa` | Frequência (diária/semanal/mensal), intervalo e dias da semana |
| `RecorrenciaPagamento` | Dia de vencimento e frequência de pagamento |
| `DinheiroPagamento` | Valor monetário com moeda (padrão BRL) |

### Domain Events

| Event | Quando é emitido |
|---|---|
| `UsuarioCriadoEvent` | Ao criar um novo usuário |
| `UsuarioAtivadoEvent` | Ao ativar a conta |
| `UsuarioAtualizadoEvent` | Ao atualizar nome ou avatar |
| `UsuarioDesativadoEvent` | Ao desativar a conta |
| `TarefaConcluidaEvent` | Ao concluir uma tarefa — recalcula próxima ocorrência |
| `PagamentoRealizadoEvent` | Ao pagar uma despesa — recalcula próximo vencimento |

---

## 📁 Estrutura de pastas

```
Favly.Application/
└── Usuarios/
    ├── Commands/
    │   ├── CriarUsuario/         ← Command + Handler + Validator
    │   ├── AtivarUsuario/
    │   ├── AtualizarUsuario/
    │   └── DesativarUsuario/
    ├── Queries/
    │   ├── ObterUsuarioPorId/    ← Query + Handler
    │   └── ObterUsuarioPorEmail/
    └── DTOs/
        ├── UsuarioResponse.cs
        └── UsuarioRequests.cs

Favly.Domain/
├── Entities/
├── Events/
├── Interfaces/          # IUsuarioRepository, IPasswordHasher, ITokenService, IUnitOfWork
├── ValueObjects/
└── Common/
    ├── Base/            # Entity, AggregateRoot, ValueObject, IDomainEvent
    ├── Enums/
    ├── Exceptions/      # DomainException, NotFoundException
    └── Validations/     # Guard

Favly.Infrastructure/
├── Data/
│   ├── FavlyDbContext.cs
│   ├── Configurations/  # IEntityTypeConfiguration por entidade
│   └── Migrations/
├── Repositories/
├── Security/            # BcryptPasswordHasher, TokenService
├── Persistence/         # UnitOfWork
└── Extensions/          # InfrastructureExtensions, ApplicationExtensions

Tests/
├── Favly.Tests.Unit/
│   ├── Domain/          # Testes das entidades
│   └── Application/     # Testes dos handlers
└── Favly.Tests.Integration/
    ├── Helpers/         # FavlyWebFactory, UsuarioFaker
    └── Integration/     # Testes end-to-end com banco real
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
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_JWT_COM_MAIS_DE_32_CARACTERES" --project Favly.api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=favly_db;Username=postgres;Password=SUA_SENHA;SSL Mode=Disable" --project Favly.api
```

### 3. Suba o banco com Docker

```bash
docker-compose up favly-db -d
```

### 4. Rode a aplicação

```bash
dotnet run --project Favly.api --launch-profile http
```

### URLs disponíveis

| Modo | URL |
|---|---|
| `dotnet run` (http) | `http://localhost:5019` |
| `dotnet run` (https) | `https://localhost:7243` |
| Docker | `http://localhost:8083` |

O **Swagger** abre automaticamente na raiz: `http://localhost:5019`

### Ou rode tudo com Docker Compose

```bash
docker-compose up --build
```

---

## 🔐 Autenticação

A API usa **JWT Bearer Token**. Para acessar endpoints protegidos:

1. `POST /api/usuarios` — cria o usuário
2. `POST /api/usuarios/{id}/ativar` — ativa a conta com o código recebido
3. `POST /api/auth/login` — retorna o token JWT
4. Use o token no header: `Authorization: Bearer {token}`

---

## 📌 Endpoints

### Auth
| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/auth/login` | ❌ | Autenticar e obter token JWT |

### Usuários
| Método | Rota | Auth | Descrição |
|---|---|---|---|
| `POST` | `/api/usuarios` | ❌ | Criar novo usuário |
| `POST` | `/api/usuarios/{id}/ativar` | ❌ | Ativar conta com código |
| `GET` | `/api/usuarios/{id}` | ✅ | Obter usuário por ID |
| `PUT` | `/api/usuarios/{id}` | ✅ | Atualizar nome e avatar |
| `DELETE` | `/api/usuarios/{id}` | ✅ | Desativar usuário |

> Endpoints de Grupos, Tarefas e Pagamentos em desenvolvimento.

---

## 🧪 Testes

```bash
# Todos os testes
dotnet test

# Apenas unitários
dotnet test Tests/Favly.Tests.Unit

# Apenas integração (requer Docker)
dotnet test Tests/Favly.Tests.Integration
```

Os testes de integração sobem um **PostgreSQL real via Testcontainers** — o Docker precisa estar rodando.

---

## 🔄 CI/CD

Todo PR para `develop` ou `master` executa automaticamente via **GitHub Actions**:

- ✅ Build do projeto
- ✅ Testes unitários
- ✅ Testes de integração

O merge só é permitido após todos os checks passarem.

---

## 📄 Licença

Projeto desenvolvido para fins de aprendizado e uso pessoal.
