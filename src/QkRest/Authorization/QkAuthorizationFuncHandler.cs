using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using QkRest.Contracts;

namespace QkRest.Authorization
{
    internal class QkAuthorizationFuncHandler : IQkAuthorizationHandler
    {
        private readonly Func<FilterContext, ClaimsPrincipal> authorize;

        public QkAuthorizationFuncHandler(Func<FilterContext, ClaimsPrincipal> authorize)
        {
            this.authorize = authorize;
        }

        public ClaimsPrincipal CreatePrincipal(FilterContext context)
        {
            return authorize(context);
        }
    }
}
