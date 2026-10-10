hackatonxp
=========

Backend base for the hackathon project hackatonxp. Provides a lightweight 360° client overview API using ASP.NET Core and EF Core.

Arquitetura
- Domain: entidades e regras de negócio
- Application: casos de uso, DTOs e contratos
- Infrastructure: EF Core, DbContext, repositórios, seed e adaptadores
- Api: controllers e configuração da aplicação
- Tests: testes unitários xUnit

Pré-requisitos
- .NET 10 SDK
- Docker (para executar PostgreSQL localmente)

Rodando PostgreSQL com Docker Compose
1. Copie .env.example para .env e ajuste os valores locais.
2. docker compose up -d
3. docker compose ps
4. docker compose logs postgres

Notas sobre conexão
- Defina ConnectionStrings__Postgres no .env ou em appsettings.json para apontar o banco.
- Por padrão a aplicação exige PostgreSQL para executar em modo não-ephemeral. Para executar localmente sem Postgres, defina HACKATONXP_ALLOW_INMEMORY=true no .env (apenas para desenvolvimento).

EF Core Migrations
-- Exemplo para criar a migration inicial (ajuste caminhos conforme a solução):
dotnet ef migrations add InitialCreate \
  --project . \
  --startup-project . \
  --output-dir Infrastructure/Data/Migrations

Executando a aplicação
- dotnet restore
- dotnet build
- dotnet run

Executando testes
- dotnet test

Endpoint principal
- GET /api/clients/{id:guid}/overview

Dados de demonstração
- Seed adiciona um cliente demo com CPF sintético (não real) para testes locais.

Integrações externas
- A base inclui contratos para provedores externos (IExternalProfileProvider) e um adaptador de demonstração. Não há integrações reais configuradas.
