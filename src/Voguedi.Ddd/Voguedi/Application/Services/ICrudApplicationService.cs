using System.Threading.Tasks;
using Voguedi.Application.DataObjects;

namespace Voguedi.Application.Services
{
    public interface ICrudApplicationService<TDataObject, in TIdentity, in TCreateDataObject, in TModifyDataObject> : IApplicationService
        where TDataObject : class, IDataObject<TIdentity>
        where TCreateDataObject : class
        where TModifyDataObject : class
    {
        #region Methods

        TDataObject Create(TCreateDataObject createDataObject);

        Task<TDataObject> CreateAsync(TCreateDataObject createDataObject);

        void Delete(TIdentity id);

        Task DeleteAsync(TIdentity id);

        void Modify(TIdentity id, TModifyDataObject modifyDataObject);

        Task ModifyAsync(TIdentity id, TModifyDataObject modifyDataObject);

        TDataObject Find(TIdentity id);

        Task<TDataObject> FindAsync(TIdentity id);

        #endregion
    }

    public interface ICrudApplicationService<TDataObject, in TIdentity> : ICrudApplicationService<TDataObject, TIdentity, TDataObject, TDataObject> where TDataObject : class, IDataObject<TIdentity> { }
}
