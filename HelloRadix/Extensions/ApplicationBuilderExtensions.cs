using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace HelloRadix.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId(configuration["Swagger:ClientId"]);
                c.OAuthRealm(configuration["AzureAD:ClientId"]);
                c.OAuthScopeSeparator(" ");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelloRadix");
                c.OAuthScopes($"api://{configuration["AzureAd:ClientId"]}/{configuration["Swagger:Scope"]}");
            });
        }
    }
}