namespace Voguedi.Application.DataObjects
{
    public class DataObject<TIdentity> : IDataObject<TIdentity>
    {
        #region Ctors

        public DataObject() { }

        public DataObject(TIdentity id) => Id = id;

        #endregion

        #region IDataObject<TIdentity>

        public TIdentity Id { get; set; }

        #endregion
    }
}
