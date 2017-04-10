using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Model;

namespace QkRest.Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddQkRest();

            //*** Exception Handling ***
            //Override exception handling
            //services.AddQkRest(options => options.ExceptionHandler<MyExceptionHandler>());

            //*** Swagger ***
            //Swagger is enabled out of the box.
            //There are a few ConfigureSwagger methods that you can use to customize swagger generation.
            //ConfigureSwagger methods can be called multiple times.
            services.AddQkRest(options =>
            {
                options.ConfigureSwagger("Sample App" /* ... */);
                //options.ConfigureSwagger(new Info
                //{
                //    Title = "Sample App",
                //    TermsOfService = "Very restrictive!"
                //    /* ... */
                //});
                
                //Configure swagger using Swashbuckle options directly.
                //options.ConfigureSwagger(swashOptions => { /* ... */ });

                //This method completely overrides Qk swagger setup:
                //options.OverrideQkSwagger(swashOptions => { /* configure swagger using Swashbuckle options.*/ });

                //Shows "Authorization" token header field for non-[AllowAnonymous] APIs:
                //options.EnableSwaggerAuthorizationTokenField();
            });

            //Disable Qk Swagger. (For instance if you want to use Swashbuckle directly).
            //services.AddQkRest(options => options.DisableSwagger());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Override default Swashbuckle swagger middleware paths and what's not.
            //app.UseQkRest(suppressUseSwagger: true);
            //app.UseSwagger("swagger/v1/swagger.json");
            //app.UseSwaggerUi("my-swagger/ui");

            app.UseQkRest();
            app.UseMvc();
        }
    }
}
