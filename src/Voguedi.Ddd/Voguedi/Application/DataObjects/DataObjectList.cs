using System.Collections.Generic;

namespace Voguedi.Application.DataObjects
{
    public class DataObjectList<TDataObject, TIdentity> : IDataObjectList<TDataObject, TIdentity>
        where TDataObject : class, IDataObject<TIdentity>
    {
        #region Ctors

        public DataObjectList() { }

        public DataObjectList(IReadOnlyList<TDataObject> dataObjects) => DataObjects = dataObjects;

        #endregion

        #region IDataObjectList<TDataObject, TIdentity>

        public IReadOnlyList<TDataObject> DataObjects { get; set; }

        #endregion
    }
}
