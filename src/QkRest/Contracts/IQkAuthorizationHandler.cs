using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QkRest.Contracts
{
    /// <summary>
    /// Authorization handler is needed to create ClaimsPrincipal 
    /// from request context or return null if can't authorize request.
    /// </summary>
    public interface IQkAuthorizationHandler
    {
        /// <summary>
        /// Authorizes the request and creates ClaimsPrincipal if possible or returns null.
        /// </summary>
        ClaimsPrincipal CreatePrincipal(FilterContext context);
    }
}
