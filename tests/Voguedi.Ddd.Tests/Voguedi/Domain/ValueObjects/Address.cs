namespace Voguedi.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        #region Public Fields

        public static readonly Address Empty = new Address(null, null, null, null, null);

        #endregion

        #region Ctors

        public Address() { }

        public Address(string country, string state, string city, string street, string zip)
        {
            Country = country;
            State = state;
            City = city;
            Street = street;
            Zip = zip;
        }

        #endregion

        #region Public Properties

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Zip { get; set; }

        #endregion

        #region Public Methods

        public override string ToString() => $"[Country = {Country}, State = {State}, City = {City}, Street = {Street}, Zip = {Zip}]";

        #endregion
    }
}
