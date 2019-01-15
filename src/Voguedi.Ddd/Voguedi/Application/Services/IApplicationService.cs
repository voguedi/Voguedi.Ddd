using System.Threading.Tasks;
using Voguedi.Application.DataObjects;
using Voguedi.AspectCore;
using Voguedi.DependencyInjection;

namespace Voguedi.Application.Services
{
    public interface IApplicationService<TDataObject, in TIdentity, in TCreateDataObject, in TModifyDataObject> : IScopedDependency
        where TDataObject : class, IDataObject<TIdentity>
        where TCreateDataObject : class
        where TModifyDataObject : class
    {
        #region Methods

        TDataObject Create([NotNull] TCreateDataObject createDataObject);

        Task<TDataObject> CreateAsync([NotNull] TCreateDataObject createDataObject);

        void Delete([NotNull] TIdentity id);

        Task DeleteAsync([NotNull] TIdentity id);

        void Modify([NotNull] TIdentity id, [NotNull] TModifyDataObject modifyDataObject);

        Task ModifyAsync([NotNull] TIdentity id, [NotNull] TModifyDataObject modifyDataObject);

        TDataObject Find([NotNull] TIdentity id);

        Task<TDataObject> FindAsync([NotNull] TIdentity id);

        #endregion
    }

    public interface IApplicationService<TDataObject, in TIdentity> : IApplicationService<TDataObject, TIdentity, TDataObject, TDataObject> where TDataObject : class, IDataObject<TIdentity> { }
}
