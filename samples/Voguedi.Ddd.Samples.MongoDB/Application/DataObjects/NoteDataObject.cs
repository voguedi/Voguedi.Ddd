using Voguedi.Application.DataObjects;

namespace Voguedi.Ddd.Samples.MongoDB.Application.DataObjects
{
    public class NoteDataObject : DataObject<string>
    {
        #region Public Properties

        public string Title { get; set; }

        public string Content { get; set; }

        #endregion
    }
}
