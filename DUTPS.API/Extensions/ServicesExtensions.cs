using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DUTPS.API.Extensions
{
    internal class ApiVersionFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            string[] forbiddenNames = { "api_version", "x-requestid", "x-apikey" };
            var parametersToRemove = operation.Parameters.Where(x => forbiddenNames.Any(y => x.Name == y)).ToList();
            foreach (var parameter in parametersToRemove)
                operation.Parameters.Remove(parameter);
        }
    }
    
    public static class ServicesExtensions
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class SwaggerExcludeAttribute : Attribute
        {
        }
        public class SwaggerParameterFilter : IOperationFilter
        {
            void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var ignoredProperties = context.MethodInfo.GetParameters()
                    .SelectMany(p => p.ParameterType.GetProperties().Where(prop => prop.GetCustomAttribute<SwaggerExcludeAttribute>() != null));

                if (ignoredProperties.Any())
                {
                    foreach (var property in ignoredProperties)
                    {
                        operation.Parameters = operation.Parameters.Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCulture)).ToList();
                    }
                }
            }
        }
    }
}