using System.Threading.Tasks;

namespace Voguedi.Events
{
    public class SampleEventHandler1 : IEventHandler<SampleEvent>
    {
        #region IEventHandler<SampleEvent1>

        public Task HandleAsync(SampleEvent e)
        {
            e.Name = "SampleEventHandler";
            return Task.CompletedTask;
        }

        #endregion
    }

    public class SampleEventHandler2 : IEventHandler<SampleEvent>
    {
        #region IEventHandler<SampleEvent1>

        public Task HandleAsync(SampleEvent e)
        {
            e.Count++;
            return Task.CompletedTask;
        }

        #endregion
    }
}
