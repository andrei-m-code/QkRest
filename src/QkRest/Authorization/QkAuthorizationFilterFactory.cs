using Microsoft.AspNetCore.Mvc.Filters;
using QkRest.Contracts;
using System;

namespace QkRest.Authorization
{
    /// <summary>
    /// Factory for creating Qk authorization filter and injecting necessary dependencies.
    /// </summary>
    public class QkAuthorizationFilterFactory : IFilterFactory
    {
        /// <summary>
        /// Is reusable.
        /// </summary>
        public bool IsReusable => false;

        /// <summary>
        /// Creates Qk authorization filter and injects necessary dependencies.
        /// </summary>
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var authorizationHandler = (IQkAuthorizationHandler) serviceProvider.GetService(typeof(IQkAuthorizationHandler));
            return new QkAuthorizationFilter(authorizationHandler);
        }
    }
}
