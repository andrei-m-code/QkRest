using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Application;
using QkRest.Authorization;
using QkRest.Contracts;
using QkRest.Swagger;

namespace QkRest
{
    /// <summary>
    /// QkRest configuration options container.
    /// </summary>
    public class QkOptions
    {
        private readonly IServiceCollection services;

        internal bool exceptionsConfigured;
        internal bool authorizationConfigured;

        internal bool disableSwagger;
        internal bool swaggerConfigured;
        internal List<Action<SwaggerGenOptions>> swaggerSetup = new List<Action<SwaggerGenOptions>>();
        private Info swaggerInfo = new Info
        {
            Version = "v1",
            Title = PlatformServices.Default.Application.ApplicationName
        };

        internal QkOptions(IServiceCollection services)
        {
            this.services = services;
            swaggerSetup.Add(DefaultSwaggerConfiguration);
        }

        /// <summary>
        /// Registers exception handler.
        /// </summary>
        public void ExceptionHandler<TExceptionHandler>() where TExceptionHandler : class, IQkExceptionHandler
        {
            services.AddScoped<IQkExceptionHandler, TExceptionHandler>();
            exceptionsConfigured = true;
        }

        /// <summary>
        /// Registers authorization handler.
        /// </summary>
        public void AuthorizationHandler<TAuthorizationHandler>() where TAuthorizationHandler : class, IQkAuthorizationHandler
        {
            services.AddScoped<IQkAuthorizationHandler, TAuthorizationHandler>();
            authorizationConfigured = true;
        }

        /// <summary>
        /// Registers simple Func authorization handler.
        /// </summary>
        public void AuthorizationHandler(Func<FilterContext, ClaimsPrincipal> authorize)
        {
            services.AddSingleton<IQkAuthorizationHandler>(new QkAuthorizationFuncHandler(authorize));
            authorizationConfigured = true;
        }

        /// <summary>
        /// Disables Qk Swagger all-together.
        /// </summary>
        public void DisableSwagger()
        {
            disableSwagger = true;
            swaggerConfigured = true;
        }

        /// <summary>
        /// Configures swagger using native Swashbuckle options method. It completely overrides QkRest swagger setup.
        /// </summary>
        public void OverrideQkSwagger(Action<SwaggerGenOptions> setup)
        {
            swaggerSetup.Clear();
            swaggerSetup.Add(setup);
            swaggerConfigured = true;
        }

        /// <summary>
        /// Configure swagger. Customization happens after initial Qk swagger configuration.
        /// </summary>
        public void ConfigureSwagger(Action<SwaggerGenOptions> setup)
        {
            swaggerSetup.Add(setup);
            swaggerConfigured = true;
        }

        /// <summary>
        /// Sets basic API info.
        /// </summary>
        /// <param name="info"></param>
        public void ConfigureSwagger(Info info)
        {
            swaggerSetup.Add(options => options.SingleApiVersion(info));
            swaggerInfo = info;
            swaggerConfigured = true;
        }

        /// <summary>
        /// Sets basic API info.
        /// </summary>
        public void ConfigureSwagger(string title, string description = "", string terms = "", Contact contact = null, License license = null)
        {
            swaggerInfo.Title = title;
            swaggerInfo.Description = description;
            swaggerInfo.Contact = contact;
            swaggerInfo.License = license;
            swaggerInfo.TermsOfService = terms;
            swaggerSetup.Add(options => options.SingleApiVersion(swaggerInfo));
            swaggerConfigured = true;
        }

        /// <summary>
        /// Shows "Authorization" token header field for non-[AllowAnonymous] APIs.
        /// </summary>
        public void EnableSwaggerAuthorizationTokenField()
        {
            if (!swaggerConfigured || disableSwagger)
            {
                throw new InvalidOperationException("Swagger is not configured.");
            }

            services.ConfigureSwaggerGen(options => options.OperationFilter<QkAuthorizationOperationFilter>());
        }

        private void DefaultSwaggerConfiguration(SwaggerGenOptions opts)
        {
            //options.OperationFilter<SwaggerApiKeyFilter>();
            opts.SingleApiVersion(swaggerInfo);

            Directory
                .GetFiles(PlatformServices.Default.Application.ApplicationBasePath)
                .Where(file => file.EndsWith(".xml"))
                .ToList()
                .ForEach(path =>
                {
                    try
                    {
                        opts.IncludeXmlComments(path);
                    }
                    catch
                    {
                        // ignored
                    }
                });
        }        
    }
}
