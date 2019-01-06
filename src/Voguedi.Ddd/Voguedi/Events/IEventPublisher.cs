using System.Threading.Tasks;

namespace Voguedi.Events
{
    public interface IEventPublisher
    {
        #region Methods

        Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent;

        #endregion
    }
}
