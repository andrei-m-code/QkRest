using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QkRest.Authorization;
using QkRest.Contracts;
using System;
using System.Security.Claims;

namespace QkRest
{
    public class QkOptions : IConfigureOptions<MvcOptions>
    {
        private readonly IServiceCollection services;

        internal bool exceptionsConfigured;
        internal bool authorizationConfigured;

        internal QkOptions(IServiceCollection services)
        {
            this.services = services;
        }

        public void ExceptionHandler<TExceptionHandler>() where TExceptionHandler : class, IQkExceptionHandler
        {
            services.AddScoped<IQkExceptionHandler, TExceptionHandler>();
            exceptionsConfigured = true;
        }

        public void AuthorizationHandler<TAuthorizationHandler>() where TAuthorizationHandler : class, IQkAuthorizationHandler
        {
            services.AddScoped<IQkAuthorizationHandler, TAuthorizationHandler>();
            authorizationConfigured = true;
        }

        public void AuthorizationHandler(Func<FilterContext, ClaimsPrincipal> authorize)
        {
            services.AddSingleton<IQkAuthorizationHandler>(new QkAuthorizationFuncHandler(authorize));
            authorizationConfigured = true;
        }

        public void Configure(MvcOptions options)
        {
            if (authorizationConfigured)
            {
                options.Filters.Add(new QkAuthorizationFilterFactory());
            }
        }
    }
}
