using System;
using System.IO;
using System.Reflection;
using Gateway.API.Helpers;
using Gateway.Data.Repository;
using Gateway.Data.Services;
using Gateway.Interfaces.Repository;
using Gateway.Interfaces.Services;
using Gateway.Shared.Interfaces;
using Gateway.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Gateway.API.Extensions
{
    public static class ServiceConfigExtensions
    {
        public static IServiceCollection AddSystemServices(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Gateway API",
                        Version = "v1",
                        Description = "Basic Gateway using .NET Core 3.1",
                        Contact = new OpenApiContact
                        {
                            Name = "Marcu Iulian",
                            Email = "julian_marcu@yahoo.com",
                            Url = new Uri("https://www.linkedin.com/in/iulian-marcu-282003a9/"),
                        },
                    });

#if Debug
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
#endif
                });


            services.AddScoped<IGatewayRepository, GatewayRepository>();
            services.AddScoped<IRepositoryService,RepositoryService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICardDetailsService, CardDetailsService>();
            services.AddScoped<IMerchantService,MerchantService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<ITransactionDetailsValidatorService, TransactionDetailsValidatorService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<IWebRequestService, WebRequestService>();


            return services;
        }
    }
}