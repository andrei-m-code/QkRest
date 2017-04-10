using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using QkRest.Contracts;
using QkRest.Exceptions;

namespace QkRest.Authorization
{
    /// <summary>
    /// Qk MVC Authorization filter.
    /// </summary>
    public class QkAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IQkAuthorizationHandler authorizationHandler;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationHandler">Authorization handler with logic for authorizing requests/users.</param>
        public QkAuthorizationFilter(IQkAuthorizationHandler authorizationHandler)
        {
            this.authorizationHandler = authorizationHandler;
        }

        /// <summary>
        /// Called when authorization is required for MVC action.
        /// </summary>
        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            context.HttpContext.User = authorizationHandler.CreatePrincipal(context);

            if (context.HttpContext.User == null && !context.Filters.Any(filter => filter is AllowAnonymousFilter))
            {
                throw new QkUnauthorizedException("User is not authorized.");
            }
        }
    }
}
