using System;
using Voguedi.Utils;

namespace Voguedi.Events
{
    public abstract class Event : IEvent
    {
        #region Ctors

        protected Event()
        {
            Id = ObjectId.NewObjectId().ToString();
            Timestamp = DateTime.UtcNow;
        }

        #endregion

        #region IEvent

        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        #endregion
    }
}
