using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Office4U.Data.Ef.SqlServer.Contexts;
using Office4U.Domain.Model.Articles.Entities;
using Office4U.Domain.Model.Users.Entities;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Office4U.Data.Ef.SqlServer.SeedData
{
    public static class Seed
    {
        public static async Task SeedUsers(
              UserManager<AppUser> userManager,
              RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + "/_SeedData/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole> {
                new AppRole() {Name="User"},
                new AppRole() {Name="Admin"},
                new AppRole() {Name="ManageArticles"},
                new AppRole() {Name="ImportArticles"},
                new AppRole() {Name="ExportArticles"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "User");
            }

            var admin = new AppUser { UserName = "admin" };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin" });
        }

        public static async Task SeedArticles(CommandDbContext context)
        {
            if (await context.Articles.AnyAsync()) return;

            var articleData = await System.IO.File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + "/_SeedData/ArticleSeedData.json");
            var articlePhotoData = await System.IO.File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + "/_SeedData/ArticlePhotoSeedData.json");

            var articles = JsonSerializer.Deserialize<List<Article>>(articleData);
            var articlePhotos = JsonSerializer.Deserialize<List<ArticlePhoto>>(articlePhotoData);

            foreach (var article in articles)
            {
                context.Articles.Add(article);
            }

            await context.SaveChangesAsync();

            foreach (var articlePhoto in articlePhotos)
            {
                context.ArticlePhotos.Add(articlePhoto);
            }

            await context.SaveChangesAsync();
        }
    }
}