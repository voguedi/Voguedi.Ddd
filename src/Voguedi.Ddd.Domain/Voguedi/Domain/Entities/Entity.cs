using System.Collections.Generic;

namespace Voguedi.Domain.Entities
{
    public abstract class Entity<TIdentity> : ValueObject, IEntity<TIdentity>
    {
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
