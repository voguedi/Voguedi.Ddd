using System.Threading.Tasks;

namespace Voguedi.Events
{
    public interface IEventHandler { }

    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : class, IEvent
    {
        #region Methods

        Task HandleAsync(TEvent e);

        #endregion
    }
}
