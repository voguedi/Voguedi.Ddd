using Xunit;

namespace Voguedi.Domain.ValueObjects
{
    public class ValueObjectTests
    {
        #region Public Methods

        [Fact]
        public void ValueObject_Equals_SameData()
        {
            var address1 = new Address("Country", "State", "City", "Street", "Zip");
            var address2 = new Address("Country", "State", "City", "Street", "Zip");

            Assert.Equal(address1, address2);
            Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
            Assert.True(address1.Equals(address2));
            Assert.True(address1 == address2);
            Assert.False(address1 != address2);

            var address3 = Address.Empty;
            var address4 = Address.Empty;
            Assert.Equal(address3.GetHashCode(), address4.GetHashCode());
        }

        [Fact]
        public void ValueObject_NotEquals_DifferentData()
        {
            var address1 = new Address("Country1", "State", "City", "Street", "Zip");
            var address2 = new Address("Country2", "State", "City", "Street", "Zip");

            Assert.NotEqual(address1, address2);
            Assert.NotEqual(address1.GetHashCode(), address2.GetHashCode());
            Assert.False(address1.Equals(address2));
            Assert.False(address1 == address2);
            Assert.True(address1 != address2);

            Assert.NotEqual(
                new Address("Country", "State1", "City", "Street", "Zip"),
                new Address("Country", "State2", "City", "Street", "Zip"));

            Assert.NotEqual(
                new Address("Country", "State", "City1", "Street", "Zip"),
                new Address("Country", "State", "City2", "Street", "Zip"));

            Assert.NotEqual(
                new Address("Country", "State", "City", "Street1", "Zip"),
                new Address("Country", "State", "City", "Street2", "Zip"));

            Assert.NotEqual(
                new Address("Country", "State", "City", "Street", "Zip1"),
                new Address("Country", "State", "City", "Street", "Zip2"));
        }

        [Fact]
        public void ValueObject_NotEquals_DerivedClass()
        {
            var address1 = new Address("Country", "State", "City", "Street", "Zip");
            var derivedAddress1 = new DerivedAddress
            {
                City = "City",
                Country = "Country",
                State = "State",
                Street = "Street",
                Zip = "Zip"
            };

            Assert.False(address1.Equals(derivedAddress1));
        }

        [Fact]
        public void ValueObject_NotEquals_Null()
        {
            var address1 = new Address("Country", "State", "City", "Street", "Zip");

            Assert.False(address1.Equals(null));
            Assert.True(address1 != null);
        }

        [Fact]
        public void ValueObject_Equals_BothNull()
        {
            Address address1 = null;
            Address address2 = null;
            Assert.True(address1 == address2);
        }

        #endregion
    }
}
