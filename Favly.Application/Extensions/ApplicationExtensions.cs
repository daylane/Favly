using Favly.Application.Usuarios.Commands.CriarUsuario;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Favly.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CriarUsuarioValidator>();

            return services;
        }
    }
}
