using Microsoft.AspNetCore.Mvc.Filters;
using QkRest.Contracts;
using System;

namespace QkRest.Authorization
{
    public class QkAuthorizationFilterFactory : IFilterFactory
    {
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var authorizationHandler = (IQkAuthorizationHandler) serviceProvider.GetService(typeof(IQkAuthorizationHandler));
            return new QkAuthorizationFilter(authorizationHandler);
        }
    }
}
