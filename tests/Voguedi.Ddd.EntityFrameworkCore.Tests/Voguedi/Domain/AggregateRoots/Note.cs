using Voguedi.Utils;

namespace Voguedi.Domain.AggregateRoots
{
    public class Note : AggregateRoot<long>
    {
        #region Ctors

        public Note() { }

        public Note(long id, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;
        }

        public Note(string title, string content) : this(SnowflakeId.Instance.NewId(), title, content) { }

        #endregion

        #region Public Properties

        public string Title { get; set; }

        public string Content { get; set; }

        #endregion
    }
}
