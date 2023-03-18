using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace dls_SqlServerQueryMapper_Test.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            return services
                .AddVersionedApiExplorer()
                .AddApiVersioning(setup =>
                {
                    setup.AssumeDefaultVersionWhenUnspecified = true;
                    setup.DefaultApiVersion = new ApiVersion(1, 0);
                    setup.ApiVersionReader = ApiVersionReader.Combine(
                        new HeaderApiVersionReader()
                        {
                            HeaderNames = { "x-api-version" }
                        },
                        new QueryStringApiVersionReader()
                        {
                            ParameterNames = { "api-version" }
                        });
                });
        }

        public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                var apiDescriptionProvider = services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var apiVersionDescription in apiDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        apiVersionDescription.GroupName,
                        new OpenApiInfo
                        {
                            Contact = new OpenApiContact { Email = "diego.loira@gmail.com" },
                            Version = apiVersionDescription.ApiVersion.ToString(),
                            Title = $"Swagger {apiVersionDescription.ApiVersion.ToString()}",
                            TermsOfService = default,
                            Description = "A collection of endpints to test the database query mapper",
                            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") },
                        }
                    );
                }
            });
        }

        public static IServiceCollection AddObservability(this IServiceCollection services, ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            return services;
        }
    }
}
