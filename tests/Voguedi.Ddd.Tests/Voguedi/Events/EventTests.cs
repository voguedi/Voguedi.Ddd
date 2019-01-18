using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Voguedi.Events
{
    public class EventTests
    {
        #region Public Methods

        [Fact]
        public async Task Event_HandleAsync()
        {
            var publisher = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IEventPublisher, EventPublisher>()
                .AddTransient<IEventHandler<SampleEvent>, SampleEventHandler1>()
                .BuildServiceProvider()
                .GetRequiredService<IEventPublisher>();
            var sampleEvent = new SampleEvent();
            await publisher.PublishAsync(sampleEvent);

            Assert.True(sampleEvent.Name == "SampleEventHandler");
        }

        [Fact]
        public async Task Event_MultipleHandleAsync()
        {
            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IEventPublisher, EventPublisher>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEventHandler<SampleEvent>, SampleEventHandler1>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEventHandler<SampleEvent>, SampleEventHandler2>());
            var publisher = services
                .BuildServiceProvider()
                .GetRequiredService<IEventPublisher>();
            var sampleEvent = new SampleEvent();
            await publisher.PublishAsync(sampleEvent);

            Assert.True(sampleEvent.Name == "SampleEventHandler");
            Assert.True(sampleEvent.Count == 1);
        }

        #endregion
    }
}
