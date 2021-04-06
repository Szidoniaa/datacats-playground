using HelloRadix.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

namespace HelloRadix
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration,
                jwtBearerScheme: JwtBearerDefaults.AuthenticationScheme,
                subscribeToJwtBearerMiddlewareDiagnosticsEvents: true);

            services.RegisterServices();

            services.AddDataProtection();

            services.ConfigureSwagger(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.ConfigureSwagger(Configuration);

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();

            });

            app.Run(handler: context => { context.Response.Redirect("swagger/index.html"); return System.Threading.Tasks.Task.CompletedTask; });

        }
    }
}