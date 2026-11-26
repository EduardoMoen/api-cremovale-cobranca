using System.Text;
using CobrancaEscolar.Common;
using CobrancaEscolar.Data;
using CobrancaEscolar.DTOs.Andamento;
using CobrancaEscolar.DTOs.Lugar;
using CobrancaEscolar.DTOs.PosicaoCheque;
using CobrancaEscolar.DTOs.PosicaoContrato;
using CobrancaEscolar.Services;
using CobrancaEscolar.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CobrancaEscolar.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.EnableRetryOnFailure(3)
            ));
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtKey = config["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicies.TemEscritorio, policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim("escritorio_id"));

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<IAuthService, AuthService>();

        // SuperUser only
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IEscritorioService, EscritorioService>();
        services.AddScoped<AlineaService>();
        services.AddScoped<BancoService>();
        services.AddScoped<TipoCobrancaService>();
        services.AddScoped<IndiceService>();

        // Multitenancy services
        services.AddScoped<IEntidadeService, EntidadeService>();
        services.AddScoped<IEscolaService, EscolaService>();
        services.AddScoped<IResponsavelService, ResponsavelService>();
        services.AddScoped<IDividaService, DividaService>();

        // Escritório-scoped generic services
        services.AddScoped<
            IEscritorioScopedService<AndamentoDto, CreateAndamentoDto, UpdateAndamentoDto, AndamentoFilterDto>,
            AndamentoService>();
        services.AddScoped<
            IEscritorioScopedService<LugarDto, CreateLugarDto, UpdateLugarDto, LugarFilterDto>,
            LugarService>();
        services.AddScoped<
            IEscritorioScopedService<PosicaoChequeDto, CreatePosicaoChequeDto, UpdatePosicaoChequeDto, PosicaoChequeFilterDto>,
            PosicaoChequeService>();
        services.AddScoped<
            IEscritorioScopedService<PosicaoContratoDto, CreatePosicaoContratoDto, UpdatePosicaoContratoDto, PosicaoContratoFilterDto>,
            PosicaoContratoService>();

        return services;
    }

    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
    {
        // Usa o OpenAPI nativo do .NET 10 (Microsoft.AspNetCore.OpenApi)
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = "Sistema de Cobrança Escolar API";
                document.Info.Version = "v1";
                document.Info.Description = "API REST para o Sistema de Cobrança Escolar";
                return Task.CompletedTask;
            });
        });

        return services;
    }
}
