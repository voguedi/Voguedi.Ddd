using System;
using System.Collections.Generic;

namespace Voguedi
{
    public class VoguediOptions
    {
        #region Private Fields

        readonly List<IServiceRegistrar> serviceRegistrars = new List<IServiceRegistrar>();

        #endregion

        #region Public Properties

        public IReadOnlyList<IServiceRegistrar> ServiceRegistrars => serviceRegistrars;

        #endregion

        #region Public Methods

        public void Register(IServiceRegistrar serviceRegistrar)
        {
            if (serviceRegistrar == null)
                throw new ArgumentNullException(nameof(serviceRegistrar));

            serviceRegistrars.Add(serviceRegistrar);
        }

        #endregion
    }
}
