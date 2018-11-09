using System;
using Microsoft.Extensions.DependencyInjection;
using AutoMapperMapperConfigurationExpression = AutoMapper.Configuration.MapperConfigurationExpression;

namespace Voguedi
{
    class AutoMapperServiceRegistrar : IServiceRegistrar
    {
        #region Private Fields

        readonly Action<AutoMapperMapperConfigurationExpression> setupAction;

        #endregion

        #region Ctors

        public AutoMapperServiceRegistrar(Action<AutoMapperMapperConfigurationExpression> setupAction) => this.setupAction = setupAction;

        #endregion

        #region IServiceRegistrar

        public void Register(IServiceCollection services) => services.AddAutoMapper(setupAction);

        #endregion
    }
}
