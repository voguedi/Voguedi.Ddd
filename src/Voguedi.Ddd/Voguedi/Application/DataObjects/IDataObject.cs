namespace Voguedi.Application.DataObjects
{
    public interface IDataObject<TIdentity>
    {
        #region Properties

        TIdentity Id { get; set; }

        #endregion
    }
}
