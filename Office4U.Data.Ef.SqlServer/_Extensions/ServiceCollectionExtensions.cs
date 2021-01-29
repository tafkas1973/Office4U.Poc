using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Data.Ef.SqlServer.SeedData;
using Office4U.Domain.Model.Users.Entities;
using System;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReadOnlyDataContext>(options =>
            {
                // TODO: read project configuration
                // options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=Office4U.Article;Trusted_Connection=True;");
            });
            services.AddDbContext<DataContext>(options =>
            {
                // options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=Office4U.Article;Trusted_Connection=True;");
            });
        }

        public async static Task SeedDatabase(this IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(userManager, roleManager);
                await Seed.SeedArticles(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<DataContext>>();
                logger.LogError(ex, "An error occured during migration");
            }
        }
    }
}
