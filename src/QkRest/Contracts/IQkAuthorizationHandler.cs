using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QkRest.Contracts
{
    public interface IQkAuthorizationHandler
    {
        ClaimsPrincipal CreatePrincipal(FilterContext context);
    }
}
