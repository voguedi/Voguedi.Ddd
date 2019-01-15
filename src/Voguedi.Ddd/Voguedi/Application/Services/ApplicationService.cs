using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Voguedi.Application.DataObjects;
using Voguedi.Domain.AggregateRoots;
using Voguedi.Domain.Repositories;
using Voguedi.ObjectMapping;

namespace Voguedi.Application.Services
{
    public abstract class ApplicationService<TAggregateRoot, TDataObject, TIdentity, TCreateDataObject, TModifyDataObject>
        : IApplicationService<TDataObject, TIdentity, TCreateDataObject, TModifyDataObject>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
        where TDataObject : class, IDataObject<TIdentity>
        where TCreateDataObject : class
        where TModifyDataObject : class
    {
        #region Protected Fields

        protected IRepositoryContext RepositoryContext;
        protected IRepository<TAggregateRoot, TIdentity> Repository;
        protected IObjectMapper ObjectMapper;

        #endregion

        #region Ctors

        protected ApplicationService(IRepositoryContext repositoryContext, IObjectMapper objectMapper)
        {
            RepositoryContext = repositoryContext;
            Repository = repositoryContext.GetRepository<TAggregateRoot, TIdentity>();
            ObjectMapper = objectMapper;
        }

        #endregion

        #region Protected Methods

        protected virtual TAggregateRoot MapToAggregateRoot(TCreateDataObject createDataObject)
            => createDataObject != null ? ObjectMapper.Map<TCreateDataObject, TAggregateRoot>(createDataObject) : null;

        protected virtual TAggregateRoot MapToAggregateRoot(TModifyDataObject modifyDataObject, TAggregateRoot aggregateRoot)
            => modifyDataObject != null ? ObjectMapper.Map(modifyDataObject, aggregateRoot) : null;

        protected virtual TDataObject MapToDataObject(TAggregateRoot aggregateRoot)
            => aggregateRoot != null ? ObjectMapper.Map<TAggregateRoot, TDataObject>(aggregateRoot) : null;

        protected virtual IReadOnlyList<TDataObject> CreateRange(IReadOnlyList<TCreateDataObject> createDataObjects)
        {
            if (createDataObjects == null)
                throw new ArgumentNullException(nameof(createDataObjects));

            if (createDataObjects.Count > 0)
            {
                var aggregateRoots = createDataObjects.Select(dto => MapToAggregateRoot(dto));

                foreach (var aggregateRoot in aggregateRoots)
                    Repository.Create(aggregateRoot);

                RepositoryContext.Commit();
                return aggregateRoots.Select(agg => MapToDataObject(agg)).ToList();
            }

            return null;
        }

        protected virtual async Task<IReadOnlyList<TDataObject>> CreateRangeAsync(IReadOnlyList<TCreateDataObject> createDataObjects)
        {
            if (createDataObjects == null)
                throw new ArgumentNullException(nameof(createDataObjects));

            if (createDataObjects.Count > 0)
            {
                var aggregateRoots = createDataObjects.Select(dto => MapToAggregateRoot(dto));

                foreach (var aggregateRoot in aggregateRoots)
                    await Repository.CreateAsync(aggregateRoot);

                await RepositoryContext.CommitAsync();
                return aggregateRoots.Select(agg => MapToDataObject(agg)).ToList();
            }

            return null;
        }

        protected virtual void DeleteRange(IReadOnlyList<TIdentity> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            if (ids.Count > 0)
            {
                var aggregateRoot = default(TAggregateRoot);

                foreach (var id in ids)
                {
                    aggregateRoot = Repository.Find(id);

                    if (aggregateRoot != null)
                        Repository.Delete(aggregateRoot);
                }

                RepositoryContext.Commit();
            }
        }

        protected virtual async Task DeleteRangeAsync(IReadOnlyList<TIdentity> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            if (ids.Count > 0)
            {
                var aggregateRoot = default(TAggregateRoot);

                foreach (var id in ids)
                {
                    aggregateRoot = await Repository.FindAsync(id);

                    if (aggregateRoot != null)
                        await Repository.DeleteAsync(aggregateRoot);
                }

                await RepositoryContext.CommitAsync();
            }
        }

        protected virtual void ModifyRange(IReadOnlyList<(TIdentity id, TModifyDataObject modifyDataObject)> modifyDataObjectMapping)
        {
            if (modifyDataObjectMapping == null)
                throw new ArgumentNullException(nameof(modifyDataObjectMapping));

            if (modifyDataObjectMapping.Count > 0)
            {
                foreach (var (id, modifyDataObject) in modifyDataObjectMapping)
                    Repository.Modify(MapToAggregateRoot(modifyDataObject, Repository.Find(id)));

                RepositoryContext.Commit();
            }
        }

        protected virtual async Task ModifyRangeAsync(IReadOnlyList<(TIdentity id, TModifyDataObject modifyDataObject)> modifyDataObjectMapping)
        {
            if (modifyDataObjectMapping == null)
                throw new ArgumentNullException(nameof(modifyDataObjectMapping));

            if (modifyDataObjectMapping.Count > 0)
            {
                foreach (var (id, modifyDataObject) in modifyDataObjectMapping)
                    Repository.Modify(MapToAggregateRoot(modifyDataObject, await Repository.FindAsync(id)));

                await RepositoryContext.CommitAsync();
            }
        }

        protected virtual IQueryable<TAggregateRoot> GetAll() => Repository.GetAll();

        #endregion

        #region IApplicationService<TDataObject, TIdentity, TCreateDataObject, TModifyDataObject>

        public virtual TDataObject Create(TCreateDataObject createDataObject)
        {
            var aggregateRoot = MapToAggregateRoot(createDataObject);
            Repository.Create(aggregateRoot);
            RepositoryContext.Commit();
            return MapToDataObject(aggregateRoot);
        }

        public virtual async Task<TDataObject> CreateAsync(TCreateDataObject createDataObject)
        {
            var aggregateRoot = MapToAggregateRoot(createDataObject);
            await Repository.CreateAsync(aggregateRoot);
            await RepositoryContext.CommitAsync();
            return MapToDataObject(aggregateRoot);
        }

        public virtual void Delete(TIdentity id)
        {
            var aggregateRoot = Repository.Find(id);

            if (aggregateRoot != null)
            {
                Repository.Delete(aggregateRoot);
                RepositoryContext.Commit();
            }
        }

        public virtual async Task DeleteAsync(TIdentity id)
        {
            var aggregateRoot = await Repository.FindAsync(id);

            if (aggregateRoot != null)
            {
                await Repository.DeleteAsync(aggregateRoot);
                await RepositoryContext.CommitAsync();
            }
        }

        public virtual TDataObject Find(TIdentity id) => MapToDataObject(Repository.Find(id));

        public virtual async Task<TDataObject> FindAsync(TIdentity id) => MapToDataObject(await Repository.FindAsync(id));

        public virtual void Modify(TIdentity id, TModifyDataObject modifyDataObject)
        {
            Repository.Modify(MapToAggregateRoot(modifyDataObject, Repository.Find(id)));
            RepositoryContext.Commit();
        }

        public virtual async Task ModifyAsync(TIdentity id, TModifyDataObject modifyDataObject)
        {
            await Repository.ModifyAsync(MapToAggregateRoot(modifyDataObject, Repository.Find(id)));
            await RepositoryContext.CommitAsync();
        }

        #endregion
    }

    public abstract class ApplicationService<TAggregateRoot, TDataObject, TIdentity>
        : ApplicationService<TAggregateRoot, TDataObject, TIdentity, TDataObject, TDataObject>
        , IApplicationService<TDataObject, TIdentity>
        where TAggregateRoot : class, IAggregateRoot<TIdentity>
        where TDataObject : class, IDataObject<TIdentity>
    {
        #region Ctors

        protected ApplicationService(IRepositoryContext repositoryContext, IObjectMapper objectMapper) : base(repositoryContext, objectMapper) { }

        #endregion
    }
}
