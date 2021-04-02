using System;
using System.Collections.Generic;
using Collibra.Converters;
using Collibra.HttpClients;
using Collibra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace HelloRadix.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<ICollibraService, CollibraService>();
            services.AddTransient<ICommunitiesConverter, CommunitiesConverter>();
            services.AddSingleton<ICollibraHttpClient, CollibraHttpClient>();

            services.AddHttpClient();
        }

        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Scheme = "Bearer",
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                            Scopes = new Dictionary<string, string>
                            {
                                {$"api://{Configuration["AzureAd:ClientId"]}/{Configuration["Swagger:Scope"]}", Configuration["Swagger:Scope"]}
                            },
                            TokenUrl = new Uri($"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token")
                        }
                    }
                });
                c.CustomSchemaIds(i => i.FullName);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

            });
        }
    }
}