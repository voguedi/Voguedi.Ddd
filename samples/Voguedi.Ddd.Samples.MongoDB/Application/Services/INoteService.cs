using Voguedi.Application.Services;
using Voguedi.Ddd.Samples.MongoDB.Application.DataObjects;
using Voguedi.Application.DataObjects;
using System.Threading.Tasks;

namespace Voguedi.Ddd.Samples.MongoDB.Application.Services
{
    public interface INoteService : IApplicationService<NoteDataObject, string, NoteCreateDataObject, NoteModifyDataObject>
    {
        #region Methods

        Task<PagedDataObjectList<NoteDataObject, string>> FindAllAsync(string title = null, string content = null, int pageNumber = 1, int pageSize = 10);

        #endregion
    }
}
