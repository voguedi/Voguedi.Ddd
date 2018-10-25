using Voguedi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class VoguediOptionsExtensions
    {

        #region Public Methods

        public static VoguediOptions UseEventBus(this VoguediOptions options)
        {
            options.Register(new EventBusServiceRegistrar());
            return options;
        }

        #endregion
    }
}
