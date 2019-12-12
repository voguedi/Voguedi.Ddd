namespace Voguedi.Domain.Repositories
{
    public interface IRepositoryContext : IRepositoryProvider, IUnitofWork
    {
        #region Properties

        object DbContext { get; }

        #endregion
    }

    public interface IRepositoryContext<TDbContext> : IRepositoryContext
        where TDbContext : class
    {
        #region Properties

        new TDbContext DbContext { get; }

        #endregion
    }
}
