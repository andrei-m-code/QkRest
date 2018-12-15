using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QkRest.Swagger
{
    internal class QkAuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AllowAnonymousFilter);

            if (!allowAnonymous)
            {
                operation.Parameters = operation.Parameters ?? new List<IParameter>();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    Description = "Authorization token.",
                    Type = "string",
                    In = "header",
                    Required = true
                });
            }
        }
    }
}
