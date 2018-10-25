using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voguedi.Events;

namespace Voguedi.Ddd.Samples.MongoDB.Events.Handlers
{
    public class NoteLogEventHandler : IEventHandler<NoteLogEvent>
    {
        #region Private Fields

        readonly ILogger logger;

        #endregion

        #region Ctors

        public NoteLogEventHandler(ILogger<NoteLogEventHandler> logger) => this.logger = logger;

        #endregion

        #region IEventHandler<NoteLogEvent>

        public Task HandleAsync(NoteLogEvent e)
        {
            logger.LogDebug($"[NoteId = {e.DataObject.Id}, NoteTitle = {e.DataObject.Title}, NoteContent = {e.DataObject.Content}]");
            return Task.CompletedTask;
        }

        #endregion
    }
}
