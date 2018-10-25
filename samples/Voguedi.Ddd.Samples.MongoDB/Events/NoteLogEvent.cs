using Voguedi.Ddd.Samples.MongoDB.Application.DataObjects;
using Voguedi.Ddd.Samples.MongoDB.Domain.Model;
using Voguedi.Events;

namespace Voguedi.Ddd.Samples.MongoDB.Events
{
    public class NoteLogEvent : Event
    {
        #region Public Properties

        public NoteDataObject DataObject { get; set; }

        #endregion

        #region Ctors

        public NoteLogEvent() { }


        public NoteLogEvent(NoteDataObject dataObject) => DataObject = dataObject;

        #endregion
    }
}
