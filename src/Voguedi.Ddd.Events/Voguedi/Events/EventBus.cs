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
    class EventBus : IEventBus
    {
        #region Private Fields

        readonly IServiceProvider serviceProvider;
        readonly ILogger logger;
        readonly ConcurrentDictionary<Type, IEnumerable<IEventHandler>> handlerMapping = new ConcurrentDictionary<Type, IEnumerable<IEventHandler>>();

        #endregion

        #region Ctors

        public EventBus(IServiceProvider serviceProvider, ILogger<EventBus> logger)
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
                logger.LogInformation( $"事件处理器 [{handler.GetType()}] 执行成功！ [EventType = {e.GetType()}, EventId = {e.Id}]");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"事件处理器 [{handler.GetType()}] 执行失败！ [EventType = {e.GetType()}, EventId = {e.Id}]");
            }
        }

        #endregion

        #region IEventBus

        async Task IEventBus.PublishAsync<TEvent>(TEvent e)
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
                    logger.LogWarning($"事件 [Type = {typeof(TEvent)}, Id = {e.Id}] 未注册任何处理器！");
            }
        }

        #endregion
    }
}
