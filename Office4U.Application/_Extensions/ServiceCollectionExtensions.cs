﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Office4U.Data.Ef.SqlServer.Repositories;
using Office4U.Data.Ef.SqlServer.UnitOfWork;
using Office4U.ReadApplication.Articles.Interfaces;
using Office4U.ReadApplication.Articles.Interfaces.IOC;
using Office4U.ReadApplication.Articles.Queries;
using Office4U.ReadApplication.Users.Interfaces;
using Office4U.ReadApplication.Users.Interfaces.IOC;
using Office4U.ReadApplication.Users.Queries;
using Office4U.WriteApplication.Articles.Commands;
using Office4U.WriteApplication.Articles.Interfaces;
using Office4U.WriteApplication.Articles.Interfaces.IOC;
using Office4U.WriteApplication.Helpers;
using Office4U.WriteApplication.Interfaces.IOC;

namespace Office4U.WriteApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            // readonly app
            services.AddScoped<IReadOnlyArticleRepository, ReadOnlyArticleRepository>();
            services.AddScoped<IReadOnlyUserRepository, ReadOnlyUserRepository>();

            services.AddScoped<IGetArticlesQuery, GetArticlesQuery>();
            services.AddScoped<IGetArticleQuery, GetArticleQuery>();
            services.AddScoped<IGetUsersQuery, GetUsersQuery>();
            services.AddScoped<IGetUserQuery, GetUserQuery>();

            // write app
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            
            services.AddScoped<ICreateArticleCommand, CreateArticleCommand>();
            services.AddScoped<IUpdateArticleCommand, UpdateArticleCommand>();
            services.AddScoped<IDeleteArticleCommand, DeleteArticleCommand>();
        }
    }
}
