using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Voguedi.Events
{
    class EventPublisher : IEventPublisher
    {
        #region Private Fields

        readonly IServiceProvider serviceProvider;
        readonly ILogger logger;
        readonly ConcurrentDictionary<Type, IEnumerable<IEventHandler>> handlerMapping = new ConcurrentDictionary<Type, IEnumerable<IEventHandler>>();

        #endregion

        #region Ctors

        public EventPublisher(IServiceProvider serviceProvider, ILogger<EventPublisher> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        #endregion

        #region Private Methods

        IEnumerable<IEventHandler> GetHandlers(Type eventType, IServiceScope serviceScope)
        {
            return handlerMapping.GetOrAddIfNotNull(
                eventType,
                key =>
                {
                    var handlerType = typeof(IEventHandler<>).GetTypeInfo().MakeGenericType(eventType);
                    return serviceScope.ServiceProvider.GetServices(handlerType)?.Cast<IEventHandler>();
                });
        }

        async Task HandleAsync(IEvent e, IEventHandler handler)
        {
            try
            {
                var handlerMethod = handler.GetType().GetTypeInfo().GetMethod("HandleAsync", new[] { e.GetType() });
                await (Task)handlerMethod.Invoke(handler, new object[] { e });
                logger.LogInformation( $"事件处理器执行成功！ [EventType = {e.GetType()}, EventId = {e.Id}, EventHandlerType = {handler.GetType()}]");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"事件处理器执行失败！ [EventType = {e.GetType()}, EventId = {e.Id}, EventHandlerType = {handler.GetType()}]");
            }
        }

        #endregion

        #region IEventPublisher

        async Task IEventPublisher.PublishAsync<TEvent>(TEvent e)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var handlers = GetHandlers(typeof(TEvent), serviceScope);
                
                if (handlers?.Count() > 0)
                {
                    foreach (var handler in handlers)
                        await HandleAsync(e, handler);
                }
                else
                    logger.LogError($"事件未注册任何处理器！ [EventType = {typeof(TEvent)}, EventId = {e.Id}] ");
            }
        }

        #endregion
    }
}
