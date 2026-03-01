# Projeto para o teste de .NET da LAR

API em .NET 9 para cadastro de `Pessoa` e `Telefone`, estruturada em camadas e com foco em separacao de responsabilidades, regras de dominio e testes automatizados.

## Tecnologias

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core 9 (SQLite)
- FluentValidation
- Mapster
- Scrutor
- Swashbuckle (Swagger)
- NUnit + AwesomeAssertions + Bogus
- NetArchTest.Rules (testes de arquitetura)

## Arquitetura e organizacao

### Estrutura de projetos

- `src/Lar.TesteDotNet.Domain`
  - Entidades, Value Objects, validacoes de dominio e erros.
- `src/Lar.TesteDotNet.Application`
  - Casos de uso com Commands/Queries, handlers, validadores e paginacao.
- `src/Lar.TesteDotNet.Infrastructure`
  - Persistencia com EF Core, `AppDbContext`, configuracoes e repositorio.
- `src/Lar.TesteDotNet.Presentation`
  - API (controllers), DI, configuracoes, swagger e tratamento global de excecao.
- `src/Lar.TesteDotNet.Shared`
  - Interfaces compartilhadas (Mediator, Repository, UnitOfWork, mapeamento) e wrappers.
- `tests/*`
  - Testes de dominio, integracao, arquitetura e utilitarios de testes.

### Fluxo em alto nivel

1. O Controller recebe a requisicao HTTP.
2. O Controller chama o `IMediator`.
3. O Mediator resolve o handler de Command/Query.
4. Behaviors aplicam validacao (FluentValidation) antes do handler executar.
5. O handler usa `IUnitOfWork` + `IRepository<T>` para ler/escrever dados.
6. Entidades de dominio executam regras e retornam `DomainResult`.
7. O retorno da aplicacao (`RequestResult`) e convertido para `ApiResponse`.

## Patterns aplicados

### 1) TDD (na pratica do repositorio)

- O projeto possui suite de testes automatizados cobrindo:
  - regras de dominio (`Pessoa`, `Telefone`, `Cpf`, `Validator`, `DomainResult`);
  - comportamento da API (fluxo completo com banco SQLite em memoria);
  - regras de dependencia entre camadas (testes de arquitetura).

### 2) CQRS

- Separacao de leitura e escrita por feature:
  - `Handlers/Pessoas/Commands/*`
  - `Handlers/Pessoas/Queries/*`
  - `Handlers/Telefones/Commands/*`
  - `Handlers/Telefones/Queries/*`
- Commands alteram estado.
- Queries retornam dados (inclusive com paginacao por cursor).

### 3) Mediator

- Implementacao propria de `IMediator` em `Application/Messaging/Mediator.cs`.
- Handlers sao resolvidos por DI e executados via `SendCommandAsync`/`SendQueryAsync`.

### 4) Pipeline de validacao (FluentValidation)

- Validators por command/query.
- Registro automatico via `AddValidatorsFromAssembly`.
- Decorators com Scrutor:
  - `ValidationCommandBehavior<,>`
  - `ValidationQueryBehavior<,>`
- Regras de existencia e unicidade usando `UnitOfWork`/repositorios.

### 5) Repository Pattern + Unit of Work

- Interface generica `IRepository<T>` com operacoes de consulta e escrita.
- `UnitOfWork` centraliza repositorios e `CommitAsync`.
- `AppDbContext` encapsula persistencia com EF Core.

### 6) Mapeamento de entidades com Mapster

- DTOs implementam `IMapFrom<T>`.
- Configuracao centralizada em `Presentation/Configurations/Mapping/MappingConfiguration.cs`.
- Conversoes de entidade -> DTO em handlers de query.

### 7) DDD tatico (foco em dominio)

- Entidades com comportamento e invariantes:
  - `Pessoa` (`Nome`, `Cpf`, `DataNascimento`, `Ativo`)
  - `Telefone` (`Tipo`, `Numero`, `PessoaId`)
- Value Object `Cpf` com validacao de digitos verificadores.
- `DomainResult` para sucesso/falha no dominio.

### Observacao importante

Atualmente, no startup da API, quando o provider e SQLite, a rotina de bootstrap executa `EnsureDeleted()` + `EnsureCreated()`.  
Na pratica, os dados sao recriados ao subir a aplicacao (comportamento util para os testes).

## Como executar

### Pre-requisitos

- SDK .NET 9 instalado.
- Docker (opcional, para execucao em container).

### Execucao local

```bash
dotnet restore
dotnet build Lar.TesteDotNet.sln
dotnet run --project src/Lar.TesteDotNet.Presentation
```

> Ao executar por `dotnet run` na raiz da `.sln`, informe o projeto startup com `--project`.

### Swagger local

- `http://localhost:5215/swagger`
- `https://localhost:7029/swagger`

### Execucao via Docker

O `Dockerfile` da aplicacao esta em `build/Dockerfile`.

1. Build da imagem:

```bash
docker build -f build/Dockerfile -t lar-testedotnet:latest .
```

2. Subir container:

```bash
docker run -d --rm \
  -p 5215:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ConnectionStrings__DefaultConnection="Data Source=/app/data/lar.testedotnet.db" \
  -v lar_testedotnet_data:/app/data \
  --name lar-testedotnet-api \
  lar-testedotnet:latest
```

3. Acessar Swagger no container:

- `http://localhost:5215/swagger`

### Execucao via scripts

- Linux/macOS:

```bash
./build/run-docker.sh
```

- Windows PowerShell:

```powershell
./build/run-docker.ps1
```

Ambos os scripts fazem build da imagem e sobem o container.

Variaveis/parametros suportados:

- `IMAGE_NAME` / `-ImageName`
- `CONTAINER_NAME` / `-ContainerName`
- `HOST_PORT` / `-HostPort`
- `ASPNETCORE_ENVIRONMENT` / `-AspNetCoreEnvironment`
- `DB_VOLUME` / `-DbVolume`

### Observacao sobre SQLite no container

A rotina atual de inicializacao para SQLite executa `EnsureDeleted()` + `EnsureCreated()` a cada subida da API.  
Mesmo com volume Docker, o banco e recriado a cada inicializacao (comportamento intencional para o contexto de teste tecnico).

## CI/CD (GitHub Actions)

Pipeline criada em:

- `.github/workflows/ci-cd-self-hosted.yml`

Fluxo:

1. `CI` no runner `self-hosted`: restore, build e testes da solucao.
2. `CD` no runner `self-hosted` (apenas em `push` para `main`): build da imagem Docker e deploy em modo detach usando `./build/run-docker.sh`.
3. Healthcheck no endpoint do Swagger para validar publicacao.

Variaveis opcionais de configuracao (Repository Variables):

- `APP_IMAGE_NAME` (default: `lar-testedotnet`)
- `APP_CONTAINER_NAME` (default: `lar-testedotnet-api`)
- `APP_HOST_PORT` (default: `5215`)
- `APP_ENVIRONMENT` (default: `Development`)
- `APP_DB_VOLUME` (default: `lar_testedotnet_data`)

Pre-requisitos no runner `self-hosted`:

- Docker instalado e funcional.
- `curl` disponivel para o healthcheck.

## Testes

Executar tudo:

```bash
dotnet test Lar.TesteDotNet.sln
```

Executar por suite:

```bash
dotnet test tests/Lar.TesteDotNet.Tests.Domain/Lar.TesteDotNet.Tests.Domain.csproj
dotnet test tests/Lar.TesteDotNet.Tests.Integration/Lar.TesteDotNet.Tests.Integration.csproj
```

## Endpoints principais

### Pessoas

- `GET /api/pessoas?limit={n}&cursor={cursor}`
- `GET /api/pessoas/{id}`
- `POST /api/pessoas`
- `PUT /api/pessoas/{id}`
- `PATCH /api/pessoas/{id}/ativar`
- `PATCH /api/pessoas/{id}/desativar`

### Telefones (aninhado em Pessoa)

- `GET /api/pessoas/{pessoaId}/telefones?limit={n}&cursor={cursor}`
- `GET /api/pessoas/{pessoaId}/telefones/{id}`
- `POST /api/pessoas/{pessoaId}/telefones`
- `PUT /api/pessoas/{pessoaId}/telefones/{id}`
- `DELETE /api/pessoas/{pessoaId}/telefones/{id}`
