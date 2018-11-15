using System;

namespace Voguedi.Events
{
    public interface IEvent
    {
        #region Properties

        string Id { get; set; }

        DateTime Timestamp { get; set; }

        #endregion
    }
}
