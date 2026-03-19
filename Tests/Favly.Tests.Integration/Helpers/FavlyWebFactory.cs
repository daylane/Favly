using Favly.Domain.Interfaces;
using Favly.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Wolverine.Persistence;

namespace Favly.Tests.Integration.Helpers
{
    public class FavlyWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
           .WithImage("postgres:17")
           .WithDatabase("favly_test")
           .WithUsername("postgres")
           .WithPassword("test123")
           .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d =>
                  d.ServiceType == typeof(DbContextOptions<FavlyDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Registra com banco de teste
                services.AddDbContext<FavlyDbContext>(options =>
                    options.UseNpgsql(_postgres.GetConnectionString()));
            });
        }

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FavlyDbContext>();
            await db.Database.MigrateAsync();
        }
        public async Task ResetDatabaseAsync()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FavlyDbContext>();

            db.Usuarios.RemoveRange(db.Usuarios);
            await db.SaveChangesAsync();
        }

        public new async Task DisposeAsync()
        {
            await _postgres.StopAsync();
        }
    }
}
