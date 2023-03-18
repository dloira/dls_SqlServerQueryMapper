using dls_SqlServerQueryMapper_Migration.Migrations;
using dls_SqlServerQueryMapper_Test.Extensions;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace dls_SqlServerQueryMapper_Migration;

public class Program
{
    static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        using (var scope = CreateServices(config).CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
        }
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static ServiceProvider CreateServices(IConfiguration config)
    {
        return new ServiceCollection()
            .AddApplicationServices()
            .AddMigrations(config)
            .BuildServiceProvider(false);
    }
}


