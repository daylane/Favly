using Favly.Application.Usuarios.Commands.CriarUsuario;
using Favly.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using System.Text;

namespace Favly.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            var infrastructureAssembly = Assembly.GetAssembly(typeof(TokenService));

            services.Scan(scan => scan
                .FromAssemblies(infrastructureAssembly)
                .AddClasses(classes => classes
                    .Where(type =>
                        type.Name.EndsWith("Repository") ||
                        type.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
