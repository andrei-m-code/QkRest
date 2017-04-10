using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QkRest.Contracts;
using QkRest.Middleware;

namespace QkRest
{
    /// <summary>
    /// Extensions to enable QkRest.
    /// </summary>
    public static class QkExtensions
    {
        private static bool qkRestAdded;
        private static bool qkRestUsed;

        /// <summary>
        /// Configures and registers QkRest dependencies.
        /// </summary>
        public static void AddQkRest(this IServiceCollection services, Action<QkOptions> configurationAction = null)
        {
            if (!qkRestAdded)
            {
                var options = new QkOptions(services);

                configurationAction?.Invoke(options);

                if (!options.exceptionsConfigured)
                {
                    services.AddScoped<IQkExceptionHandler, QkExceptionHandler>();
                }

                if (options.authorizationConfigured)
                {
                    services.AddSingleton(typeof(IConfigureOptions<MvcOptions>), new QkMvcConfigureOptions(options));
                }

                if (!options.disableSwagger)
                {
                    services.AddSwaggerGen();
                    options.swaggerSetup.ForEach(services.ConfigureSwaggerGen);
                }

                qkRestAdded = true;
                return;
            }

            throw new InvalidOperationException(nameof(AddQkRest) + " should be called only once!");
        }

        /// <summary>
        /// Registers QkRest middlewares.
        /// </summary>
        public static void UseQkRest(this IApplicationBuilder app, bool suppressUseSwagger = false)
        {
            if (!qkRestUsed)
            {
                app.UseMiddleware(typeof(QkExceptionHandlingMiddleware));

                if (!suppressUseSwagger)
                {
                    app.UseSwagger();
                    app.UseSwaggerUi();
                }

                qkRestUsed = true;
                return;
            }

            throw new InvalidOperationException(nameof(UseQkRest) + " should be called only once!");
        }
    }
}
