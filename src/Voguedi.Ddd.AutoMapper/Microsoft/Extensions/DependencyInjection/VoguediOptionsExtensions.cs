using System;
using Voguedi;
using AutoMapperMapperConfigurationExpression = AutoMapper.Configuration.MapperConfigurationExpression;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VoguediOptionsExtensions
    {
        #region Public Methods

        public static VoguediOptions UseAutoMapper(this VoguediOptions options, Action<AutoMapperMapperConfigurationExpression> setupAction)
        {
            if (setupAction == null)
                throw new ArgumentNullException(nameof(setupAction));

            options.Register(new AutoMapperServiceRegistrar(setupAction));
            return options;
        }

        public static VoguediOptions UseAutoMapper(this VoguediOptions options) => options.UseAutoMapper(_ => { });

        #endregion
    }
}
