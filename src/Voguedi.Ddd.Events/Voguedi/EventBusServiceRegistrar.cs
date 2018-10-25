using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Voguedi.Events;
using Voguedi.Reflection;

namespace Voguedi
{
    class EventBusServiceRegistrar : IServiceRegistrar
    {
        #region Private Methods

        static IEnumerable<Type> GetImplementations() => TypeFinder.Instance.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IEventHandler).IsAssignableFrom(t));

        static IEnumerable<Type> GetServices(Type implementation)
            => implementation.GetTypeInfo().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

        static void AddDomainEventHandler(IServiceCollection services)
        {
            foreach (var implementation in GetImplementations())
            {
                foreach (var service in GetServices(implementation))
                    services.TryAddTransient(service, implementation);
            }
        }

        #endregion

        #region IServiceRegistrar

        public void Register(IServiceCollection services)
        {
            AddDomainEventHandler(services);
            services.TryAddSingleton<IEventBus, EventBus>();
        }

        #endregion
    }
}
