using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QkRest.Authorization;

namespace QkRest
{
    internal class QkMvcConfigureOptions : IConfigureOptions<MvcOptions>
    {
        private readonly QkOptions qkOptions;

        public QkMvcConfigureOptions(QkOptions qkOptions)
        {
            this.qkOptions = qkOptions;
        }

        public void Configure(MvcOptions options)
        {
            if (qkOptions.authorizationConfigured)
            {
                options.Filters.Add(new QkAuthorizationFilterFactory());
            }
        }
    }
}
