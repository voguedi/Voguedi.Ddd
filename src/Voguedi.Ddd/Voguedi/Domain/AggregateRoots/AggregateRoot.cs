using Voguedi.Domain.Entities;

namespace Voguedi.Domain.AggregateRoots
{
    public abstract class AggregateRoot<TIdentity> : Entity<TIdentity>, IAggregateRoot<TIdentity> { }
}
