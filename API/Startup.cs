using API.Extensions;
using API.Infrastructure;
using AutoMapper;
using DAL.Repositories;
using DTO.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.V1;
using System;
using System.Reflection;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DbContext") ?? throw new ArgumentNullException("You must add a environment variable named 'boilerplate_ConnectionStrings__Dbcontext' !");

            services
                .AddVersioning()
                .AddSwaggerDocumentation()
                .AddSecurity()
                .AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)))
                .AddContext(connectionString)
                .AddScoped<BreweryRepository>()
                .AddScoped<RepositoryBase<Beer>>()
                .AddScoped<RepositoryBase<Wholesaler>>()
                .AddControllers(options => {
                    options.Filters.Add(new CustomExceptionFilter());
                    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                })
                .AddJsonOptions(options => 
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwaggerDocumentation(provider)
                .UseSecurity()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
