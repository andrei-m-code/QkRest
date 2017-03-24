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
        public static void AddQkRest(this IServiceCollection services, Action<QkOptions> configurationAction = null)
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
        }

        public static void UseQkRest(this IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(QkExceptionHandlingMiddleware));
        }
    }
}
