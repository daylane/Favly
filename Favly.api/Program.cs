using Favly.api.Extensions;
using Favly.api.Middleware;
using Favly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Postgresql;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ResolveDependencies(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FavlyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Host.UseWolverine(opts =>
{
    // Em vez de usar o método genérico de EF, use o do Postgres 
    // passando a connection string. Ele se integra ao FavlyDbContext automaticamente.
    opts.PersistMessagesWithPostgresql(connectionString);

    // Isso ativa o Outbox especificamente para o seu contexto do EF
    opts.UseEntityFrameworkCoreOutbox<FavlyDbContext>();

    opts.Policies.AutoApplyTransactions();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "favly v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
