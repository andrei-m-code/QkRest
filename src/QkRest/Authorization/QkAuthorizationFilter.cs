using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using QkRest.Contracts;
using QkRest.Exceptions;

namespace QkRest.Authorization
{
    public class QkAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IQkAuthorizationHandler authorizationHandler;

        public QkAuthorizationFilter(IQkAuthorizationHandler authorizationHandler)
        {
            this.authorizationHandler = authorizationHandler;
        }

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.User = authorizationHandler.CreatePrincipal(context);

            if (context.HttpContext.User == null && !context.Filters.Any(filter => filter is AllowAnonymousFilter || filter is AllowAnonymousAttribute))
            {
                throw new QkUnauthorizedException("User is not authorized.");
            }
        }
    }
}
