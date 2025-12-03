# Ambev Developer Evaluation - Backend API

API RESTful desenvolvida em .NET 8.0 para gerenciamento de usuários, produtos e carrinhos de compras, seguindo os princípios de Domain-Driven Design (DDD) e Clean Architecture.

## 📋 Índice

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias e Frameworks](#tecnologias-e-frameworks)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Fluxos Implementados](#fluxos-implementados)
- [APIs Disponíveis](#apis-disponíveis)
- [Configuração e Execução](#configuração-e-execução)
- [Testes](#testes)
- [Docker](#docker)
- [Migrações](#migrações)
- [Logging](#logging)
- [Autenticação](#autenticação)

## 🎯 Visão Geral

Este projeto implementa uma API RESTful completa para gerenciamento de e-commerce, incluindo:

- **Usuários**: CRUD completo com autenticação JWT
- **Produtos**: CRUD completo com categorias e avaliações
- **Carrinhos de Compras**: CRUD completo com itens de produtos
- **Autenticação**: Sistema de autenticação baseado em JWT

A aplicação segue os princípios de **Domain-Driven Design (DDD)** e **Clean Architecture**, garantindo separação de responsabilidades, testabilidade e manutenibilidade.

## 🏗️ Arquitetura

### Padrões Arquiteturais

O projeto utiliza uma arquitetura em camadas baseada em **Clean Architecture** e **DDD**:

```
┌─────────────────────────────────────────┐
│         WebApi (Presentation)            │
│  - Controllers                           │
│  - DTOs (Requests/Responses)            │
│  - Middleware                           │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│      Application (Use Cases)             │
│  - Commands/Queries (CQRS)               │
│  - Handlers (MediatR)                    │
│  - Validators (FluentValidation)         │
│  - DTOs (Results)                        │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│         Domain (Business Logic)          │
│  - Entities                              │
│  - Value Objects                         │
│  - Domain Services                       │
│  - Repository Interfaces                 │
│  - Domain Events                         │
└─────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────┐
│      Infrastructure (ORM/Repositories)   │
│  - Entity Framework Core                 │
│  - Repository Implementations            │
│  - Database Context                      │
│  - Migrations                            │
└─────────────────────────────────────────┘
```

### Princípios Aplicados

1. **Domain-Driven Design (DDD)**
   - Entidades ricas com lógica de negócio
   - Value Objects para conceitos imutáveis
   - Repositórios como abstrações de persistência
   - External Identities para referências entre bounded contexts

2. **Clean Architecture**
   - Separação clara de responsabilidades
   - Dependências apontam para dentro (Domain no centro)
   - Testabilidade através de inversão de dependências

3. **CQRS (Command Query Responsibility Segregation)**
   - Separação entre Commands (escrita) e Queries (leitura)
   - Handlers específicos para cada operação
   - Otimização independente de leitura e escrita

4. **Mediator Pattern**
   - Desacoplamento entre controllers e handlers
   - Comunicação através de Commands/Queries
   - Pipeline behaviors para cross-cutting concerns

## 🛠️ Tecnologias e Frameworks

### Backend Core

- **.NET 8.0**: Plataforma de desenvolvimento moderna e performática
- **C# 12**: Linguagem de programação com recursos modernos
- **ASP.NET Core 8.0**: Framework web para construção de APIs RESTful

### Persistência de Dados

- **Entity Framework Core 8.0.10**: ORM para acesso a dados
- **PostgreSQL 13**: Banco de dados relacional principal
- **Npgsql.EntityFrameworkCore.PostgreSQL 8.0.8**: Provider EF Core para PostgreSQL
- **MongoDB 8.0**: Banco de dados NoSQL (configurado, não utilizado atualmente)
- **Redis 7.4.1**: Cache distribuído (configurado, não utilizado atualmente)

### Padrões e Bibliotecas

- **MediatR 12.4.1**: Implementação do padrão Mediator para CQRS
- **AutoMapper 13.0.1**: Mapeamento automático entre objetos
- **FluentValidation 11.10.0**: Validação fluente e declarativa
- **BCrypt.Net-Next 4.0.3**: Hash de senhas seguro

### Autenticação e Segurança

- **Microsoft.AspNetCore.Authentication.JwtBearer 8.0.10**: Autenticação JWT
- **JWT (JSON Web Tokens)**: Tokens para autenticação stateless

### Logging e Monitoramento

- **Serilog 8.0.3**: Framework de logging estruturado
- **Serilog.Exceptions 8.4.0**: Enriquecimento de logs com detalhes de exceções
- **Serilog.Sinks.Console 6.0.0**: Sink para console
- **Serilog.Exceptions.EntityFrameworkCore 8.4.0**: Destructuring de exceções do EF Core

### Health Checks

- **Microsoft.Extensions.Diagnostics.HealthChecks 8.0.11**: Health checks nativos
- **AspNetCore.HealthChecks.NpgSql 8.0.2**: Health check para PostgreSQL
- **AspNetCore.HealthChecks.MongoDb 8.1.0**: Health check para MongoDB
- **AspNetCore.HealthChecks.Redis 8.0.1**: Health check para Redis

### Testes

- **xUnit 2.9.2**: Framework de testes unitários
- **FluentAssertions 6.12.0**: Assertions expressivas e legíveis
- **NSubstitute 5.1.0**: Framework de mocking
- **Bogus 35.6.1**: Geração de dados fake para testes
- **Microsoft.AspNetCore.Mvc.Testing 8.0.10**: Testes de integração HTTP
- **Microsoft.EntityFrameworkCore.InMemory 8.0.10**: Banco em memória para testes

### Documentação

- **Swashbuckle.AspNetCore 6.8.1**: Geração automática de documentação Swagger/OpenAPI

## 📁 Estrutura do Projeto

```
template/backend/
├── src/
│   ├── Ambev.DeveloperEvaluation.Domain/          # Camada de Domínio
│   │   ├── Entities/                              # Entidades de negócio
│   │   │   ├── User.cs
│   │   │   ├── Product.cs
│   │   │   └── Cart.cs
│   │   ├── ValueObjects/                          # Objetos de valor
│   │   ├── Repositories/                          # Interfaces de repositórios
│   │   ├── Enums/                                 # Enumerações
│   │   ├── Validation/                            # Validadores de domínio
│   │   └── Common/                                # Classes base
│   │
│   ├── Ambev.DeveloperEvaluation.Application/     # Camada de Aplicação
│   │   ├── Users/                                 # Casos de uso de usuários
│   │   │   ├── CreateUser/
│   │   │   ├── GetUser/
│   │   │   ├── GetUsers/
│   │   │   ├── UpdateUser/
│   │   │   └── DeleteUser/
│   │   ├── Products/                              # Casos de uso de produtos
│   │   ├── Carts/                                 # Casos de uso de carrinhos
│   │   ├── Auth/                                  # Casos de uso de autenticação
│   │   └── Common/                                # Utilitários comuns
│   │
│   ├── Ambev.DeveloperEvaluation.ORM/             # Camada de Infraestrutura
│   │   ├── Repositories/                          # Implementações de repositórios
│   │   ├── Mapping/                               # Configurações do EF Core
│   │   ├── Migrations/                            # Migrações do banco de dados
│   │   └── DefaultContext.cs                      # DbContext
│   │
│   ├── Ambev.DeveloperEvaluation.WebApi/           # Camada de Apresentação
│   │   ├── Features/                               # Features organizadas por recurso
│   │   │   ├── Users/
│   │   │   ├── Products/
│   │   │   ├── Carts/
│   │   │   └── Auth/
│   │   ├── Common/                                # Classes comuns da API
│   │   ├── Middleware/                            # Middlewares customizados
│   │   └── Program.cs                             # Ponto de entrada
│   │
│   ├── Ambev.DeveloperEvaluation.Common/           # Camada Transversal
│   │   ├── Security/                               # Segurança e autenticação
│   │   ├── Logging/                               # Configuração de logging
│   │   ├── Validation/                            # Validação cross-cutting
│   │   └── HealthChecks/                          # Health checks
│   │
│   └── Ambev.DeveloperEvaluation.IoC/              # Injeção de Dependência
│       ├── DependencyResolver.cs
│       └── ModuleInitializers/                    # Inicializadores de módulos
│
└── tests/
    ├── Ambev.DeveloperEvaluation.Unit/              # Testes Unitários
    ├── Ambev.DeveloperEvaluation.Functional/       # Testes Funcionais
    └── Ambev.DeveloperEvaluation.Integration/     # Testes de Integração
```

## 🔄 Fluxos Implementados

### 1. Fluxo de Criação de Usuário

```
1. Cliente → POST /api/users
2. Controller → Valida Request (FluentValidation)
3. Controller → Mapeia Request para Command (AutoMapper)
4. Controller → Envia Command via MediatR
5. MediatR → Roteia para CreateUserHandler
6. Handler → Valida Command (FluentValidation)
7. Handler → Verifica se email já existe (Repository)
8. Handler → Mapeia Command para Entity (AutoMapper)
9. Handler → Hash da senha (BCrypt)
10. Handler → Persiste no banco (Repository)
11. Handler → Mapeia Entity para Result (AutoMapper)
12. Handler → Retorna Result
13. Controller → Mapeia Result para Response (AutoMapper)
14. Controller → Retorna 201 Created
```

### 2. Fluxo de Autenticação

```
1. Cliente → POST /api/auth
2. Controller → Valida Request
3. Controller → Mapeia Request para Command
4. Controller → Envia Command via MediatR
5. Handler → Valida Command
6. Handler → Busca usuário por email (Repository)
7. Handler → Verifica senha (BCrypt)
8. Handler → Gera JWT Token
9. Handler → Retorna Token
10. Controller → Retorna 200 OK com token
```

### 3. Fluxo de Criação de Carrinho

```
1. Cliente → POST /api/carts
2. Controller → Valida Request
3. Controller → Mapeia Request para Command
4. Controller → Envia Command via MediatR
5. Handler → Valida Command
6. Handler → Cria entidade Cart
7. Handler → Cria entidades CartItem para cada produto
8. Handler → Persiste no banco (Repository)
9. Handler → Retorna Result
10. Controller → Retorna 201 Created
```

### 4. Fluxo de Listagem Paginada

```
1. Cliente → GET /api/users?_page=1&_size=10
2. Controller → Mapeia Query Parameters para Query
3. Controller → Envia Query via MediatR
4. Handler → Aplica filtros no IQueryable
5. Handler → Aplica ordenação
6. Handler → Calcula paginação
7. Handler → Executa query no banco
8. Handler → Mapeia resultados
9. Handler → Retorna Result com paginação
10. Controller → Retorna 200 OK
```

### 5. Fluxo de Tratamento de Exceções

```
1. Exceção lançada em qualquer camada
2. ValidationExceptionMiddleware captura
3. Middleware → Loga exceção (Serilog)
4. Middleware → Mapeia para ApiResponse
5. Middleware → Retorna HTTP Status apropriado:
   - ValidationException → 400 Bad Request
   - UnauthorizedAccessException → 401 Unauthorized
   - KeyNotFoundException → 404 Not Found
   - Outras exceções → 500 Internal Server Error
```

## 🌐 APIs Disponíveis

### Usuários (`/api/users`)

- `POST /api/users` - Criar usuário
- `GET /api/users/{id}` - Obter usuário por ID
- `GET /api/users` - Listar usuários (paginado, filtrado, ordenado)
- `PUT /api/users/{id}` - Atualizar usuário
- `DELETE /api/users/{id}` - Deletar usuário

### Produtos (`/api/products`)

- `POST /api/products` - Criar produto
- `GET /api/products/{id}` - Obter produto por ID
- `GET /api/products` - Listar produtos (paginado, filtrado, ordenado)
- `GET /api/products/categories` - Listar categorias
- `GET /api/products/category/{category}` - Listar produtos por categoria
- `PUT /api/products/{id}` - Atualizar produto
- `DELETE /api/products/{id}` - Deletar produto

### Carrinhos (`/api/carts`)

- `POST /api/carts` - Criar carrinho
- `GET /api/carts/{id}` - Obter carrinho por ID
- `GET /api/carts` - Listar carrinhos (paginado, filtrado, ordenado)
- `PUT /api/carts/{id}` - Atualizar carrinho
- `DELETE /api/carts/{id}` - Deletar carrinho

### Autenticação (`/api/auth`)

- `POST /api/auth` - Autenticar usuário e obter token JWT

Para documentação detalhada de cada API, consulte:
- [Users API](/.doc/users-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Auth API](/.doc/auth-api.md)
- [General API](/.doc/general-api.md)

## ⚙️ Configuração e Execução

### Pré-requisitos

- .NET 8.0 SDK
- PostgreSQL 13+ (ou Docker)
- Visual Studio 2022 / VS Code / Rider (opcional)

### Configuração Local

1. **Clone o repositório**
   ```bash
   git clone <repository-url>
   cd template/backend
   ```

2. **Configure a connection string** em `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
     }
   }
   ```

3. **Aplique as migrações** (ou deixe a aplicação aplicar automaticamente):
   ```bash
   dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
   ```

4. **Execute a aplicação**:
   ```bash
   cd src/Ambev.DeveloperEvaluation.WebApi
   dotnet run
   ```

5. **Acesse a documentação Swagger**:
   - URL: `https://localhost:5001/swagger` (HTTPS)
   - URL: `http://localhost:5000/swagger` (HTTP)

### Variáveis de Ambiente

A aplicação suporta diferentes ambientes através de arquivos `appsettings`:

- `appsettings.json` - Configuração base
- `appsettings.Development.json` - Configuração de desenvolvimento
- `appsettings.Docker.json` - Configuração para Docker

## 🧪 Testes

O projeto possui três níveis de testes:

### Testes Unitários

Testam componentes isolados (handlers, validators, entidades).

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit/
```

**Tecnologias:**
- xUnit
- NSubstitute (mocking)
- FluentAssertions
- Bogus (dados fake)

### Testes Funcionais

Testam fluxos completos de casos de uso com banco em memória.

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Functional/
```

**Características:**
- Banco de dados em memória (EF Core InMemory)
- Testam handlers completos
- Verificam persistência de dados

### Testes de Integração

Testam a API completa via HTTP com banco em memória.

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Integration/
```

**Características:**
- WebApplicationFactory para simular servidor HTTP
- Banco de dados em memória compartilhado
- Testam endpoints completos
- Verificam status codes, respostas JSON, etc.

**Documentação detalhada:**
- [Testes Funcionais](/.doc/functional-tests.md)
- [Testes de Integração](/.doc/integration-tests.md)

## 🐳 Docker

O projeto inclui configuração Docker completa via `docker-compose.yml`.

### Serviços Disponíveis

- **ambev.developerevaluation.webapi**: API principal
- **ambev.developerevaluation.database**: PostgreSQL 13
- **ambev.developerevaluation.nosql**: MongoDB 8.0
- **ambev.developerevaluation.cache**: Redis 7.4.1

### Executar com Docker

```bash
docker-compose up -d
```

A API estará disponível em:
- HTTP: `http://localhost:8080`
- HTTPS: `https://localhost:8081`
- Swagger: `http://localhost:8080/swagger`

### Parar os containers

```bash
docker-compose down
```

## 🔄 Migrações

### Aplicação Automática

A aplicação aplica automaticamente as migrações pendentes na inicialização através do método `ApplyMigrations()` no `Program.cs`.

**Comportamento:**
- Verifica migrações pendentes
- Aplica automaticamente se houver
- Loga o processo completo
- Falha na inicialização se houver erro

### Aplicação Manual

Para aplicar migrações manualmente:

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

### Criar Nova Migração

```bash
dotnet ef migrations add NomeDaMigracao --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

## 📊 Logging

### Configuração

O projeto utiliza **Serilog** para logging estruturado com as seguintes características:

- **Nível mínimo**: Debug (captura todos os erros)
- **Filtros**: Apenas logs de Information de health checks com status 200 são filtrados
- **Sinks**: Console (colorido em Debug) e Arquivo (`logs/log-.txt`)
- **Enriquecimento**: MachineName, Environment, Application, ExceptionDetails

### Logs de Erro

Todos os erros são logados automaticamente:

- **ValidationException**: LogWarning
- **UnauthorizedAccessException**: LogWarning
- **Exceções genéricas**: LogError
- **Exceções não tratadas**: LogFatal

### Formato dos Logs

**Debug Mode:**
```
[HH:mm:ss INF] [SourceContext] Message
Exception details...
```

**Release Mode:**
```
yyyy-MM-dd HH:mm:ss.fff zzz [INF] SourceContext Message
Exception details...
```

## 🔐 Autenticação

### JWT (JSON Web Tokens)

A autenticação utiliza JWT tokens com as seguintes características:

- **Algoritmo**: HS256
- **Secret Key**: Configurável via `appsettings.json`
- **Expiração**: Configurável
- **Claims**: UserId, Email, Role

### Fluxo de Autenticação

1. Cliente envia credenciais (`POST /api/auth`)
2. Sistema valida email e senha
3. Sistema gera JWT token
4. Cliente usa token no header `Authorization: Bearer {token}`
5. Middleware valida token em requisições protegidas

### Endpoints Protegidos

Atualmente, todos os endpoints estão abertos. Para proteger endpoints específicos, adicione `[Authorize]` nos controllers ou actions.

## 📚 Documentação Adicional

- [Visão Geral](/.doc/overview.md)
- [Tech Stack](/.doc/tech-stack.md)
- [Frameworks](/.doc/frameworks.md)
- [Estrutura do Projeto](/.doc/project-structure.md)
- [Users API](/.doc/users-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Auth API](/.doc/auth-api.md)
- [General API](/.doc/general-api.md)
- [Testes Funcionais](/.doc/functional-tests.md)
- [Testes de Integração](/.doc/integration-tests.md)

## 🚀 Próximos Passos

- [ ] Implementar eventos de domínio (Domain Events)
- [ ] Adicionar suporte a Message Broker (RabbitMQ/Kafka)
- [ ] Implementar cache com Redis
- [ ] Adicionar rate limiting
- [ ] Implementar versionamento de API
- [ ] Adicionar métricas e observabilidade (Prometheus/Grafana)

## 📝 Licença

Este projeto é parte de uma avaliação de desenvolvedor da Ambev.

---

**Desenvolvido com ❤️ seguindo as melhores práticas de Clean Architecture e DDD**

