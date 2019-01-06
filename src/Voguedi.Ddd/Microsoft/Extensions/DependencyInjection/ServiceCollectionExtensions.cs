using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voguedi;
using Voguedi.Events;
using Voguedi.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        #region Private Methods

        static IEnumerable<Type> GetImplementations(Type service) => TypeFinder.Instance.GetTypes().Where(t => t.IsClass && !t.IsAbstract && service.IsAssignableFrom(t));

        static IEnumerable<Type> GetServices(Type service, Type implementation)
            => implementation.GetTypeInfo().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == service);

        static void AddEventHandler(IServiceCollection services)
        {
            foreach (var implementation in GetImplementations(typeof(IEventHandler)))
            {
                foreach (var service in GetServices(typeof(IEventHandler<>), implementation))
                    services.TryAddTransient(service, implementation);
            }
        }

        #endregion

        #region Public Methods

        public static IServiceCollection AddVoguedi(this IServiceCollection services, Action<VoguediOptions> setupAction)
        {
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));
            
            AddEventHandler(services);
            services.TryAddSingleton<IEventPublisher, EventPublisher>();

            var options = new VoguediOptions();
            setupAction(options);
            services.AddSingleton(options);

            foreach (var serviceRegistrar in options.ServiceRegistrars)
                serviceRegistrar.Register(services);

            return services;
        }

        #endregion
    }
}
