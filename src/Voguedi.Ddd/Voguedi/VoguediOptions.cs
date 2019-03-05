using System;
using System.Collections.Generic;
using System.Reflection;
using IAutoMapperMapperConfigurationExpression = AutoMapper.IMapperConfigurationExpression;

namespace Voguedi
{
    public class VoguediOptions
    {
        #region Private Fields

        readonly List<IServiceRegistrar> serviceRegistrars = new List<IServiceRegistrar>();

        #endregion

        #region Internal Properties

        internal IReadOnlyList<IServiceRegistrar> ServiceRegistrars => serviceRegistrars;
        
        internal Assembly[] Assemblies { get; set; }

        internal Action<IAutoMapperMapperConfigurationExpression>[] MapConfigs { get; set; }

        #endregion

        #region Public Methods

        public void Register(IServiceRegistrar serviceRegistrar)
        {
            if (serviceRegistrar == null)
                throw new ArgumentNullException(nameof(serviceRegistrar));

            serviceRegistrars.Add(serviceRegistrar);
        }

        public void RegisterAssemblies(params Assembly[] assemblies) => Assemblies = assemblies;

        public void RegisterAutoMapper(params Action<IAutoMapperMapperConfigurationExpression>[] mapConfigs) => MapConfigs = mapConfigs;

        #endregion
    }
}
