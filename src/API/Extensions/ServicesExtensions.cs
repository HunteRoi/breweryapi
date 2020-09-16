using System;
using API.Infrastructure;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            return services
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(b => b
                        .AllowAnyOrigin() // !! NOT SAFE !! Please consider using .WithOrigins("...") and other more constraining methods
                        .AllowAnyMethod()
                        .AllowAnyHeader());
                });
        }

        public static IServiceCollection AddContext(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            return services.AddDbContext<BreweryDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            return services
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VV";
                    options.SubstituteApiVersionInUrl = true;
                });
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(options =>
                {
                    options.OperationFilter<SwaggerDefaultValues>();
                });
        }
    }
}