using dls_SqlServerQueryMapper_Test.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dls_SqlServerQueryMapper_Test;

public class Program
{
    public static void Main(string[] args)
    {
        // Initializes the builder 
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services
            .AddObservability(builder.Logging, builder.Configuration)
            .AddApplicationOptions(builder.Configuration)
            .AddApplicationServices()
            .AddVersioning()
            .AddOpenApi()
            .AddControllers();

        // Builds the pipeline
        var app = builder.Build();

        app.UseOpenApi();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //// Configures and run de WebApplication
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseSwagger();
        //    app.UseSwaggerUI();
        //}
        //app.UseAuthorization();
        //app.MapControllers();

        app.Run();
    }
}