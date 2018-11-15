using System.Threading.Tasks;
using Voguedi.DependencyInjection;

namespace Voguedi.Events
{
    public interface IEventPublisher : ISingletonDependency
    {
        #region Methods

        Task PublishAsync<TEvent>(TEvent e) where TEvent : class, IEvent;

        #endregion
    }
}
