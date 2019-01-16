using Voguedi.Application.Services;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Application.DataObjects;
using Voguedi.Application.DataObjects;
using System.Threading.Tasks;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Application.Services
{
    public interface INoteService : ICrudApplicationService<NoteDataObject, string, NoteCreateDataObject, NoteModifyDataObject>
    {
        #region Methods

        Task<PagedDataObjectList<NoteDataObject, string>> FindAllAsync(string title = null, string content = null, int pageNumber = 1, int pageSize = 10);

        #endregion
    }
}
