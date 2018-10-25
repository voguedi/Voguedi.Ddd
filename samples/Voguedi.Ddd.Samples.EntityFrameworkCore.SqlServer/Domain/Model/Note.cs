using Voguedi.Domain.AggregateRoots;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Model
{
    public class Note : AggregateRoot<string>
    {
        #region Public Properties

        public string Title { get; set; }

        public string Content { get; set; }

        #endregion

        #region Ctors

        public Note() { }

        public Note(string title, string content)
        {
            Title = title;
            Content = content;
        }

        #endregion
    }
}
