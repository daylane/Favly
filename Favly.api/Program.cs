using Favly.api.Extensions;
using Favly.api.Middleware;
using Favly.Application.Extensions;
using Favly.Application.Usuarios.Commands.CriarUsuario;
using Favly.Infrastructure.Data;
using Favly.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);

// Permite avatares em base64 (imagens podem ser grandes)
builder.WebHost.ConfigureKestrel(options =>
    options.Limits.MaxRequestBodySize = 10 * 1024 * 1024); // 10 MB

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new Favly.api.Extensions.DecimalJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new Favly.api.Extensions.NullableDecimalJsonConverter());
    });

builder.Services.AddCors(options =>
{
    // Lê origens permitidas da config — suporta múltiplas separadas por vírgula
    // Ex: "http://localhost:3000,http://localhost:5173,https://favly.app"
    var origensConfig = builder.Configuration["App:CorsOrigins"] ?? builder.Configuration["App:FrontendUrl"] ?? "http://localhost:4200";
    var origens = origensConfig.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    options.AddPolicy("Default", policy =>
    {
        policy.WithOrigins(origens)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // necessário para cookies HttpOnly
    });
});

builder.Services.AddApplicationServices();                        
builder.Services.AddInfrastructureServices();                   
builder.Services.AddApiServices(builder.Configuration);


builder.Services.AddDbContext<FavlyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);

    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);
    opts.Discovery.IncludeAssembly(typeof(CriarUsuarioHandler).Assembly);

    opts.Policies.AddMiddleware<UnitOfWorkMiddleware>();

    if (!builder.Environment.IsEnvironment("Testing"))
    {
        opts.PersistMessagesWithPostgresql(connectionString);
        opts.UseEntityFrameworkCoreTransactions();
        opts.Policies.AutoApplyTransactions();
    }
    else
    {
        opts.Policies.AutoApplyTransactions();
    }
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FavlyDbContext>();
    db.Database.Migrate();
}

app.UseCors("Default"); // deve vir antes de UseAuthentication e UseAuthorization

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Favly v1");
        options.RoutePrefix = string.Empty;
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles(); // serve /uploads/avatars/*

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();

public partial class Program { }