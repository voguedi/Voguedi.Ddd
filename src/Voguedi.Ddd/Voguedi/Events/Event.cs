using System;
using Voguedi.IdentityGeneration;

namespace Voguedi.Events
{
    public abstract class Event : IEvent
    {
        #region Ctors

        protected Event()
        {
            Id = StringIdentityGenerator.Instance.Generate();
            Timestamp = DateTime.UtcNow;
        }

        #endregion

        #region IEvent

        public string Id { get; set; }

        public DateTime Timestamp { get; set; }

        #endregion
    }
}
