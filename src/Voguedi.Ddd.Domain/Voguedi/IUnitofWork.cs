using System;
using System.Threading.Tasks;

namespace Voguedi
{
    public interface IUnitofWork : IDisposable
    {
        #region Methods

        Task CommitAsync();

        #endregion
    }
}
