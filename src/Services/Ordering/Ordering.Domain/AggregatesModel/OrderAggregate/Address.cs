using Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork;
using System;

namespace Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public record Address : IValueObject
    {
		public Address(string street, string city, string state, string country, string zipCode)
		{
			Street = street;
			City = city;
			State = state;
			Country = country;
			ZipCode = zipCode;
		}

		public string Street { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }
    }
}
