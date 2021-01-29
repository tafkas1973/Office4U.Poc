using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Office4U.Data.Ef.SqlServer.Extensions;
using Office4U.Presentation.Controller.Services;
using Office4U.Presentation.Controller.Services.Interfaces;
using Office4U.WriteApplication.Extensions;

namespace Office4U.Presentation.Controller.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            // Application layer (Read & Write)
            services.AddScoped<ITokenService, TokenService>();
            services.RegisterApplicationServices();

            // Persistence layer
            services.RegisterDataServices(configuration);

            return services;
        }
    }
}