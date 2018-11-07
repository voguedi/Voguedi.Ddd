using System.Collections.Generic;
using Voguedi.Domain.ValueObjects;

namespace Voguedi.Domain.Entities
{
    public abstract class Entity<TIdentity> : ValueObject, IEntity<TIdentity>
    {
        #region Ctors

        protected Entity() { }

        protected Entity(TIdentity id) => Id = id;

        #endregion

        #region ValueObject

        protected override IEnumerable<object> GetEqualityPropertryValues()
        {
            yield return Id;
        }

        #endregion

        #region IEntity<TIdentity>

        public TIdentity Id { get; set; }

        #endregion
    }
}
