using System.Collections.Generic;

namespace Voguedi.Application.DataObjects
{
    public class PagedDataObjectList<TDataObject, TIdentity> : DataObjectList<TDataObject, TIdentity>, IPagedDataObjectList<TDataObject, TIdentity>
        where TDataObject : class, IDataObject<TIdentity>
    {
        #region Ctors

        public PagedDataObjectList() { }

        public PagedDataObjectList(IReadOnlyList<TDataObject> dataObjects, int pageNumber, int pageSize, int totalRecords, int totalPages)
            : base(dataObjects)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = totalPages;
        }

        public PagedDataObjectList(int pageNumber, int pageSize) : this(null, pageNumber, pageSize, 0, 0) { }

        #endregion

        #region IPagedDataObjectList<TDataObject, TIdentity>

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        #endregion
    }
}
