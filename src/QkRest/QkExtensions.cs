using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QkRest.Contracts;
using QkRest.Middleware;

namespace QkRest
{
    public static class QkExtensions
    {
        private static bool qkRestAdded;
        private static bool qkRestUsed;

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
                    services.AddSingleton(typeof(IConfigureOptions<MvcOptions>), options);
                }

                qkRestAdded = true;
                return;
            }

            throw new InvalidOperationException(nameof(AddQkRest) + " should be called only once!");
        }

        public static void UseQkRest(this IApplicationBuilder app)
        {
            if (!qkRestUsed)
            {
                app.UseMiddleware(typeof(QkExceptionHandlingMiddleware));
                qkRestUsed = true;
                return;
            }

            throw new InvalidOperationException(nameof(UseQkRest) + " should be called only once!");
        }
    }
}
