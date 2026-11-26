using CobrancaEscolar.Data;
using CobrancaEscolar.Extensions;
using CobrancaEscolar.Middleware;
using Scalar.AspNetCore;
using Serilog;

// ─── Serilog bootstrap ───────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Serilog
    builder.Host.UseSerilog((ctx, services, cfg) => cfg
        .ReadFrom.Configuration(ctx.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/cobranca-.log", rollingInterval: RollingInterval.Day));

    // ─── Services ────────────────────────────────────────────────────────────
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddDatabase(builder.Configuration);
    builder.Services.AddJwtAuthentication(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddSwaggerWithJwt();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
            policy.WithOrigins(
                    builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                    ?? ["http://localhost:4200"])
                .AllowAnyHeader()
                .AllowAnyMethod());
    });

    // ─── Pipeline ────────────────────────────────────────────────────────────
    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        // UI do Scalar disponível em /scalar/v1
        app.MapScalarApiReference(options =>
        {
            options.Title = "Sistema de Cobrança Escolar";
            options.AddPreferredSecuritySchemes("Bearer");
        });
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowFrontend");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    var urls = app.Urls.Count > 0
        ? app.Urls
        : builder.Configuration["ASPNETCORE_URLS"]?.Split(';').AsEnumerable()
          ?? ["https://localhost:7159", "http://localhost:5248"];

    // ─── Seed inicial ────────────────────────────────────────────────────────
    await DatabaseSeeder.SeedAsync(app.Services, app.Logger);

    Log.Information("Sistema de Cobrança Escolar iniciando...");
    Log.Information("Ambiente: {Env}", app.Environment.EnvironmentName);

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        foreach (var url in app.Urls)
        {
            Log.Information("Ouvindo em: {Url}", url);
        }

        if (app.Environment.IsDevelopment())
        {
            var baseUrl = app.Urls.FirstOrDefault(u => u.StartsWith("https"))
                          ?? app.Urls.FirstOrDefault()
                          ?? "https://localhost:7159";
            Log.Information("Scalar UI:   {Url}/scalar/v1", baseUrl);
            Log.Information("OpenAPI JSON:{Url}/openapi/v1.json", baseUrl);
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Falha crítica na inicialização da aplicação.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
