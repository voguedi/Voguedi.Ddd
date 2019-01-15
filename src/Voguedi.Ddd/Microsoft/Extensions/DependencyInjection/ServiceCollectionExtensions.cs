using System;
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
        
        static void AddEventHandler(IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var implementationType in new TypeFinder().GetTypesBySpecifiedType(typeof(IEventHandler<>), assemblies))
            {
                foreach (var serviceType in implementationType.GetTypeInfo().ImplementedInterfaces.Where(i => i != typeof(IEventHandler)))
                    services.TryAddEnumerable(ServiceDescriptor.Transient(serviceType, implementationType));
            }
        }

        #endregion

        #region Public Methods

        public static IServiceCollection AddVoguedi(this IServiceCollection services, Action<VoguediOptions> setupAction)
        {
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            services.TryAddSingleton<IEventPublisher, EventPublisher>();

            var options = new VoguediOptions();
            setupAction(options);

            foreach (var serviceRegistrar in options.ServiceRegistrars)
                serviceRegistrar.Register(services);

            var assemblies = options.Assemblies;
            AddEventHandler(services, assemblies);
            services.AddUitls(assemblies);
            services.AddAutoMapper(s =>
            {
                s.Assemblies = assemblies;

                if (options.MapConfigs?.Length > 0)
                    s.MapConfigs.AddRange(options.MapConfigs);
            });
            services.AddAspectCore(options.AspectConfig);
            return services;
        }

        #endregion
    }
}
