using Favly.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Favly.api.Extensions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<AppDbContext>()
            //    .AddDefaultTokenProviders();
            
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; 
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"]
                };
            });

            //var infrastructureAssembly = Assembly.GetAssembly(typeof(TokenService));

            //services.Scan(scan => scan
            //    .FromAssemblies(infrastructureAssembly)
            //    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service")))
            //    .AsImplementedInterfaces() // Registra como IClassName
            //    .WithScopedLifetime()      // Define o tempo de vida
            //);


            //services.AddIdentityCore<ApplicationUser>(options => {
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 8;
            //    options.User.RequireUniqueEmail = true;
            //}).AddRoles<IdentityRole>() 
            //.AddEntityFrameworkStores<AppDbContext>()
            //.AddDefaultTokenProviders();

            return services;
        }
    }
}
