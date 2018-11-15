using System.Collections.Generic;

namespace Voguedi.Application.DataObjects
{
    public interface IDataObjectList<TDataObject, TIdentity>
        where TDataObject : class, IDataObject<TIdentity>
    {
        #region Properties

        IReadOnlyList<TDataObject> DataObjects { get; set; }

        #endregion
    }
}
