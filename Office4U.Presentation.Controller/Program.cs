using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Office4U.Data.Ef.SqlServer.Extensions;

namespace Office4U.Presentation.Controller
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args).Build();

            using var scope = hostBuilder.Services.CreateScope();

            var services = scope.ServiceProvider;

            await services.SeedDatabase();

            await hostBuilder.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
