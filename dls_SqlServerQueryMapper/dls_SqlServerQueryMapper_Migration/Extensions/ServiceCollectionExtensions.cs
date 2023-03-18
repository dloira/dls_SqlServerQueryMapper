using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace dls_SqlServerQueryMapper_Test.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddFluentMigratorCore();
        }

        public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(configuration.GetConnectionString("sqlserver"))
                // Define the assembly containing the migrations
                //.ScanIn(typeof(AddLogTable).Assembly).For.Migrations())
                .ScanIn(Assembly.GetExecutingAssembly()).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }

        public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddObservability(this IServiceCollection services, ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            return services;
        }
    }
}
