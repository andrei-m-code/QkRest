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
            var user = authorizationHandler.CreatePrincipal(context);
            context.HttpContext.User = user ?? throw new QkUnauthorizedException("User is not authorized.");
        }
    }
}
