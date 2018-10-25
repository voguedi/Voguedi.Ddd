using System;
using System.Linq;
using System.Threading.Tasks;
using Voguedi.Application.DataObjects;
using Voguedi.Application.Services;
using Voguedi.Ddd.Samples.MongoDB.Application.DataObjects;
using Voguedi.Ddd.Samples.MongoDB.Domain.Model;
using Voguedi.Domain.Repositories;
using Voguedi.IdentityGeneration;
using Voguedi.ObjectMapping;

namespace Voguedi.Ddd.Samples.MongoDB.Application.Services
{
    class NoteService : ApplicationService<Note, NoteDataObject, string, NoteCreateDataObject, NoteModifyDataObject>, INoteService
    {
        #region Private Fields

        readonly IStringIdentityGenerator identityGenerator;

        #endregion

        #region Ctors

        public NoteService(IRepositoryContext repositoryContext, IObjectMapper objectMapper, IStringIdentityGenerator identityGenerator)
            : base(repositoryContext, objectMapper)
            => this.identityGenerator = identityGenerator;

        #endregion

        #region ApplicationService<Note, NoteDataObject, string, NoteCreateDataObject, NoteModifyDataObject>

        protected override Note MapToAggregateRoot(NoteCreateDataObject createDataObject)
        {
            var note = base.MapToAggregateRoot(createDataObject);

            if (string.IsNullOrWhiteSpace(note.Title))
                note.Title = null;

            if (string.IsNullOrWhiteSpace(note.Content))
                note.Content = null;

            note.Id = identityGenerator.Generate();
            return note;
        }

        protected override Note MapToAggregateRoot(NoteModifyDataObject modifyDataObject, Note aggregateRoot)
        {
            if (!string.IsNullOrWhiteSpace(modifyDataObject.Title))
                aggregateRoot.Title = modifyDataObject.Title;

            if (!string.IsNullOrWhiteSpace(modifyDataObject.Content))
                aggregateRoot.Content = modifyDataObject.Content;

            return aggregateRoot;
        }

        #endregion

        #region INoteService

        public Task<PagedDataObjectList<NoteDataObject, string>> FindAllAsync(string title = null, string content = null, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0)
                throw new ArgumentNullException(nameof(pageNumber));

            if (pageSize <= 0)
                throw new ArgumentNullException(nameof(pageSize));

            var notes = Repository.GetAll()
                .Where(n => (title == null || n.Title == title || n.Title.StartsWith(title) || n.Title.Contains(title) || n.Title.EndsWith(title)) &&
                            (content == null || n.Content == content || n.Content.StartsWith(content) || n.Content.Contains(content) || n.Content.EndsWith(content)))
                .OrderByDescending(n => n.Title);

            var totalRecords = notes.Count();

            if (totalRecords > 0)
            {
                var paged = notes.PageBy(pageNumber, pageSize);

                if (paged != null)
                {
                    var dataObjects = notes.Select(n => MapToDataObject(n)).ToList();
                    return Task.FromResult(new PagedDataObjectList<NoteDataObject, string>(dataObjects, pageNumber, pageSize, totalRecords, (totalRecords + pageSize - 1) / pageSize));
                }
            }

            return Task.FromResult(new PagedDataObjectList<NoteDataObject, string>(pageNumber, pageSize));
        }

        #endregion
    }
}
