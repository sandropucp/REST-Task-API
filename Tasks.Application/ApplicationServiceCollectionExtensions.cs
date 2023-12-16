using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tasks.Application.Database;
using Tasks.Application.Repositories;
using Tasks.Application.Services;

namespace Tasks.Application;
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITaskRepository, TaskRepository>();
        services.AddSingleton<ITaskService, TaskService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}