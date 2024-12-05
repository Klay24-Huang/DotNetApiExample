using Infrastrructure.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastrructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServicesFromAttributes(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes();

                if (attributes.OfType<ScopedAttribute>().Any())
                {
                    services.AddScoped(type.GetInterfaces().FirstOrDefault() ?? type, type);
                }

                if (attributes.OfType<SingletonAttribute>().Any())
                {
                    services.AddSingleton(type.GetInterfaces().FirstOrDefault() ?? type, type);
                }

                if (attributes.OfType<TransientAttribute>().Any())
                {
                    services.AddTransient(type.GetInterfaces().FirstOrDefault() ?? type, type);
                }
            }
            return services;
        }
    }
}
