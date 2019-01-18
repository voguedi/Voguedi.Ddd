using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Voguedi.Domain.AggregateRoots;
using Voguedi.Domain.Repositories.EntityFrameworkCore;
using Voguedi.Utils;
using Xunit;

namespace Voguedi.Domain.Repositories
{
    public class RepositoryTests
    {
        #region Private Methods

        IRepositoryContext ResolveRepositoryContext()
        {
            return new ServiceCollection()
                .AddDbContextPool<NoteDbContext>(o => o.UseInMemoryDatabase("RepositoryTests"))
                .AddScoped<IRepositoryContext, EntityFrameworkCoreRepositoryContext<NoteDbContext>>()
                .AddScoped<IRepositoryContext<NoteDbContext>, EntityFrameworkCoreRepositoryContext<NoteDbContext>>()
                .BuildServiceProvider()
                .GetRequiredService<IRepositoryContext>();
        }

        #endregion

        #region Public Methods

        [Fact]
        public void Repository_ResolveRepositoryContext()
        {
            var repositoryContext = ResolveRepositoryContext();

            Assert.True(repositoryContext.Id != null);
            Assert.True(repositoryContext.DbContext is NoteDbContext);
        }

        [Fact]
        public void Repository_GetRepository()
        {
            var repository = ResolveRepositoryContext().GetRepository<Note, long>();

            Assert.True(repository is IRepository<Note, long>);
        }

        [Fact]
        public void Repository_ToEntityFrameworkCoreRepository()
        {
            var repositoryContext = ResolveRepositoryContext();
            var entityFrameworkCoreRepository = repositoryContext.GetRepository<Note, long>().ToEntityFrameworkCoreRepository();
            
            Assert.True(entityFrameworkCoreRepository is IEntityFrameworkCoreRepository<Note, long>);
            Assert.True(entityFrameworkCoreRepository.DbContext is NoteDbContext);
            Assert.True(entityFrameworkCoreRepository.DbSet is DbSet<Note>);
        }

        [Fact]
        public void Repository_RepositoryContextCommit()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var note = new Note("Title", "Content");
            context.Add(note);
            repositoryContext.Commit();

            Assert.True(context.Notes.Any(n => n == note));

            context.Remove(note);
            context.SaveChanges();
        }

        [Fact]
        public async Task Repository_RepositoryContextCommitAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var note = new Note("Title", "Content");
            context.Add(note);
            await repositoryContext.CommitAsync();

            Assert.True(context.Notes.Any(n => n == note));

            context.Remove(note);
            context.SaveChanges();
        }

        [Fact]
        public void Repository_Create()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var repository = repositoryContext.GetRepository<Note, long>();
            var note = new Note(SnowflakeId.Instance.NewId(), "Title", "Content");
            repository.Create(note);
            repositoryContext.Commit();

            Assert.True(context.Notes.Any(n => n == note));

            context.Remove(note);
            context.SaveChanges();
        }

        [Fact]
        public async Task Repository_CreateAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var repository = repositoryContext.GetRepository<Note, long>();
            var note = new Note("Title", "Content");
            await repository.CreateAsync(note);
            repositoryContext.Commit();

            Assert.True(context.Notes.Any(n => n == note));

            context.Remove(note);
            context.SaveChanges();
        }

        [Fact]
        public void Repository_Delete()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var repository = repositoryContext.GetRepository<Note, long>();
            var note1 = new Note("Title1", "Content1");
            var note2 = new Note("Title2", "Content2");
            var note3 = new Note("Title3", "Content3");
            var note4 = new Note("Title3", "Content3");
            var note5= new Note("Title5", "Content5");
            repository.Create(note1);
            repository.Create(note2);
            repository.Create(note3);
            repository.Create(note4);
            repository.Create(note5);
            repositoryContext.Commit();
            repository.Delete(note1);
            repository.Delete(note2.Id);
            repository.Delete(n => n.Title == "Title3");
            repositoryContext.Commit();
            
            Assert.False(context.Notes.Any(n => n == note1));
            Assert.False(context.Notes.Any(n => n.Id == note2.Id));
            Assert.False(context.Notes.Any(n => n.Title == "Title3"));
            
            repository.Delete(note5);
            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_DeleteAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var repository = repositoryContext.GetRepository<Note, long>();
            var note1 = new Note("Title1", "Content1");
            var note2 = new Note("Title2", "Content2");
            var note3 = new Note("Title3", "Content3");
            var note4 = new Note("Title3", "Content3");
            var note5 = new Note("Title5", "Content5");
            repository.Create(note1);
            repository.Create(note2);
            repository.Create(note3);
            repository.Create(note4);
            repository.Create(note5);
            repositoryContext.Commit();
            await repository.DeleteAsync(note1);
            await repository.DeleteAsync(note2.Id);
            await repository.DeleteAsync(n => n.Title == "Title3");
            repositoryContext.Commit();

            Assert.False(context.Notes.Any(n => n == note1));
            Assert.False(context.Notes.Any(n => n.Id == note2.Id));
            Assert.False(context.Notes.Any(n => n.Title == "Title3"));

            repository.Delete(note5);
            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_Modify()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var repository = repositoryContext.GetRepository<Note, long>();
            var note1 = new Note("Title1", "Content1");
            var note2 = new Note("Title2", "Content2");
            repository.Create(note1);
            repository.Create(note2);
            repositoryContext.Commit();
            note1.Title = "Title3";
            repository.Modify(note1);
            repositoryContext.Commit();

            Assert.True(context.Notes.Count(n => n.Title == "Title3") == 1);

            repository.Delete(note1);
            repository.Delete(note2);
            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_ModifyAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var context = (NoteDbContext)repositoryContext.DbContext;
            var repository = repositoryContext.GetRepository<Note, long>();
            var note1 = new Note("Title1", "Content1");
            var note2 = new Note("Title2", "Content2");
            repository.Create(note1);
            repository.Create(note2);
            repositoryContext.Commit();
            note1.Title = "Title3";
            await repository.ModifyAsync(note1);
            repositoryContext.Commit();

            Assert.True(context.Notes.Count(n => n.Title == "Title3") == 1);

            repository.Delete(note1);
            repository.Delete(note2);
            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_GetAll()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
                notes.Add(new Note($"Title{i}", $"Content{i}"));

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            var result = repository.GetAll().OrderBy(o => o.Id);

            Assert.True(notes.OrderBy(o => o.Id).SequenceEqual(result));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_FindAll()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            var result1 = repository.FindAll().OrderBy(o => o.Id);
            var result2 = repository.FindAll(n => n.Title == "Title_X").OrderBy(o => o.Id);

            Assert.True(notes.OrderBy(o => o.Id).SequenceEqual(result1));
            Assert.True(notes.Where(n => n.Title == "Title_X").OrderBy(o => o.Id).SequenceEqual(result2));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_FindAllAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            var result1 = (await repository.FindAllAsync()).OrderBy(o => o.Id);
            var result2 = (await repository.FindAllAsync(n => n.Title == "Title_X")).OrderBy(o => o.Id);

            Assert.True(notes.OrderBy(o => o.Id).SequenceEqual(result1));
            Assert.True(notes.Where(n => n.Title == "Title_X").OrderBy(o => o.Id).SequenceEqual(result2));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_FindFirst()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(repository.FindFirst(n => n.Title == "Title_Y") == notes.FirstOrDefault(n => n.Title == "Title_Y"));
            Assert.True(repository.FindFirst(n => n.Title == "Title_2") == notes.FirstOrDefault(n => n.Title == "Title_2"));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_FindFirstAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(await repository.FindFirstAsync(n => n.Title == "Title_Y") == notes.FirstOrDefault(n => n.Title == "Title_Y"));
            Assert.True(await repository.FindFirstAsync(n => n.Title == "Title_2") == notes.FirstOrDefault(n => n.Title == "Title_2"));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_FindSingle()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(repository.FindSingle(n => n.Title == "Title_Y") == notes.SingleOrDefault(n => n.Title == "Title_Y"));
            Assert.Throws<InvalidOperationException>(() => repository.FindSingle(n => n.Title == "Title_X"));
            Assert.True(repository.FindSingle(n => n.Title == "Title_2") == notes.SingleOrDefault(n => n.Title == "Title_2"));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_FindSingleAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(await repository.FindSingleAsync(n => n.Title == "Title_Y") == notes.SingleOrDefault(n => n.Title == "Title_Y"));
            Assert.True(await repository.FindSingleAsync(n => n.Title == "Title_2") == notes.SingleOrDefault(n => n.Title == "Title_2"));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_Find()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
                notes.Add(new Note($"Title{i}", $"Content{i}"));

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(repository.Find(notes[0].Id) == notes[0]);

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_FindAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
                notes.Add(new Note($"Title{i}", $"Content{i}"));

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(await repository.FindAsync(notes[0].Id) == notes[0]);

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_CountAll()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(repository.CountAll() == notes.Count());
            Assert.True(repository.CountAll(n => n.Title == "Title_X") == notes.Where(n => n.Title == "Title_X").Count());

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_CountAllAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(await repository.CountAllAsync() == notes.Count());
            Assert.True(await repository.CountAllAsync(n => n.Title == "Title_X") == notes.Where(n => n.Title == "Title_X").Count());

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_LongCountAll()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(repository.LongCountAll() == notes.LongCount());
            Assert.True(repository.LongCountAll(n => n.Title == "Title_X") == notes.Where(n => n.Title == "Title_X").LongCount());

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_LongCountAllAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(await repository.LongCountAllAsync() == notes.LongCount());
            Assert.True(await repository.LongCountAllAsync(n => n.Title == "Title_X") == notes.Where(n => n.Title == "Title_X").LongCount());

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public void Repository_Exists()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(repository.Exists(n => n.Title == "Title_X") == notes.Any(n => n.Title == "Title_X"));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        [Fact]
        public async Task Repository_ExistsAsync()
        {
            var repositoryContext = ResolveRepositoryContext();
            var repository = repositoryContext.GetRepository<Note, long>();
            var notes = new List<Note>();

            for (var i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    notes.Add(new Note($"Title{i}", $"Content{i}"));
                else
                    notes.Add(new Note("Title_X", "Content_X"));
            }

            foreach (var note in notes)
                repository.Create(note);

            repositoryContext.Commit();

            Assert.True(await repository.ExistsAsync(n => n.Title == "Title_X") == notes.Any(n => n.Title == "Title_X"));

            foreach (var note in notes)
                repository.Delete(note);

            repositoryContext.Commit();
        }

        #endregion
    }
}
