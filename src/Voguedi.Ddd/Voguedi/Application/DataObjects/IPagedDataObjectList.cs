namespace Voguedi.Application.DataObjects
{
    public interface IPagedDataObjectList<TDataObject, TIdentity> : IDataObjectList<TDataObject, TIdentity>
        where TDataObject : class, IDataObject<TIdentity>
    {
        #region Properties

        int PageNumber { get; set; }

        int PageSize { get; set; }

        int TotalRecords { get; set; }

        int TotalPages { get; set; }

        #endregion
    }
}
