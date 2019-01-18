using Xunit;

namespace Voguedi.Domain.Entities
{
    public class EntityTests
    {
        #region Public Methods

        [Fact]
        public void Entity_Equals()
        {
            var sample1 = new EntitySample { Id = 1L, Name = "Name" };
            var sample2 = new EntitySample { Id = 1L, Name = "Name" };

            Assert.Equal(sample1, sample2);
            Assert.True(sample1 == sample2);

            var sample3 = new EntitySample { Id = 1L, Name = "Name3" };
            Assert.True(sample3 == sample1);

            var sample4 = sample1;
            sample4.Id = 2L;
            Assert.True(sample4 == sample1);
        }

        [Fact]
        public void Entity_NotEquals_DifferentId()
        {
            var sample1 = new EntitySample { Id = 1L, Name = "Name" };
            var sample2 = new EntitySample { Id = 2L, Name = "Name" };

            Assert.NotEqual(sample1, sample2);
            Assert.True(sample1 != sample2);
        }

        #endregion
    }
}
