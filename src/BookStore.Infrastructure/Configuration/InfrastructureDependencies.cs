using BookStore.Domain.Interfaces;
using BookStore.Domain.Services;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Metrics;
using BookStore.Infrastructure.Repositories;
using BookStore.Infrastructure.Repositories.Fakers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection RegisterInfrastureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            Console.WriteLine(connectionString);
            services.AddDbContext<BookStoreDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<BookStoreDbContext>();

            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryRepository, CategoryInMemoryRepository>();
            services.AddSingleton<IBookRepository, BookInMemoryRepository>();
            //services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddSingleton<OtelMetrics>();

            return services;
        }
    }
}