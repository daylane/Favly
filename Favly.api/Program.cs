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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

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

    opts.PersistMessagesWithPostgresql(connectionString);
    opts.UseEntityFrameworkCoreTransactions();
    opts.Policies.AutoApplyTransactions();
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FavlyDbContext>();
    db.Database.Migrate();
}

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

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

app.Run();
