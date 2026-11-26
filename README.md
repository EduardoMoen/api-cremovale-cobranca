# Sistema de Cobrança Escolar — Backend

## Requisitos
- .NET 10 SDK
- SQL Server (local ou Docker)

## Configuração

1. Crie o banco de dados executando os scripts em `/database/` na ordem:
   - `01_create_database.sql`
   - `02_create_tables.sql`

2. Copie `appsettings.Development.json` e ajuste:
   - `ConnectionStrings:DefaultConnection` — string de conexão com o SQL Server
   - `Jwt:Key` — chave secreta JWT (mínimo 32 caracteres)

   **Nunca versione `appsettings.Development.json` com credenciais reais.**

## Executar

```bash
cd BackEnd/CobrancaEscolar
dotnet run
```

A API ficará disponível em `https://localhost:7xxx`.  
O documento OpenAPI estará em `https://localhost:7xxx/openapi/v1.json` (ambiente Development).

## Estrutura

```
CobrancaEscolar/
├── Common/              # Utilitários (PasswordHelper, ClaimsPrincipalExtensions)
├── Controllers/         # Endpoints REST
├── Data/
│   ├── AppDbContext.cs
│   └── Configurations/  # Fluent API (IEntityTypeConfiguration)
├── DTOs/                # Data Transfer Objects por entidade
├── Entities/            # Entidades do domínio
├── Extensions/          # ServiceCollectionExtensions (DI, Auth, Swagger)
├── Middleware/          # ExceptionMiddleware (tratamento centralizado)
└── Services/            # Regras de negócio
    └── Interfaces/
```

## Autenticação

- JWT com validade de 8 horas.
- `POST /api/auth/login` → retorna o token.
- Inclua o token nas requisições: `Authorization: Bearer {token}`.
- No primeiro acesso (usuário sem senha), use `POST /api/auth/set-initial-password`.

## Regra de multitenancy

Todo endpoint que retorna dados de escritório aplica obrigatoriamente o filtro
`escritorio_id` do usuário autenticado no token JWT. Dados de outros escritórios
nunca são retornados.
