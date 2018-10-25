using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Application.DataObjects;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Model;
using Voguedi.Events;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Events
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
