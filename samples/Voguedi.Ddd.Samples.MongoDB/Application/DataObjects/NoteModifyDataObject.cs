using Voguedi.Ddd.Samples.MongoDB.Domain.Model;
using Voguedi.ObjectMapping;

namespace Voguedi.Ddd.Samples.MongoDB.Application.DataObjects
{
    [Mapper(typeof(Note), IsDestination = false)]
    public class NoteModifyDataObject
    {
        #region Public Properties

        public string Title { get; set; }

        public string Content { get; set; }

        #endregion
    }
}
