using Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ordering.UnitTests.Domain.SeedWork
{
    public class ValueObjectTests
    {
		[Theory]
		[MemberData(nameof(EqualValueObjects))]
		public void Equals_EqualValueObjects_ReturnsTrue(IValueObject instanceA, IValueObject instanceB, string reason)
		{
			// Act
			var result = EqualityComparer<IValueObject>.Default.Equals(instanceA, instanceB);

			// Assert
			Assert.True(result, reason);
		}

        [Theory]
        [MemberData(nameof(NonEqualValueObjects))]
        public void Equals_NonEqualValueObjects_ReturnsFalse(IValueObject instanceA, IValueObject instanceB, string reason)
        {
            // Act
            var result = EqualityComparer<IValueObject>.Default.Equals(instanceA, instanceB);

            // Assert
            Assert.False(result, reason);
        }

        private static readonly IValueObject APrettyValueObject = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"));

        public static readonly TheoryData<IValueObject, IValueObject, string> EqualValueObjects = new TheoryData<IValueObject, IValueObject, string>
        {
			{
				null,
				null,
				"they should be equal because they are both null"
			},
			{
				APrettyValueObject,
				APrettyValueObject,
				"they should be equal because they are the same object"
			},
			{
				new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3")),
				new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3")),
				"they should be equal because they have equal members"
			},
			{
                new ValueObjectB(1, "2",  1, 2, 3 ),
                new ValueObjectB(1, "2",  1, 2, 3 ),
                "they should be equal because all equality components are equal, including the 'C' list"
            }
        };

        public static readonly TheoryData<IValueObject, IValueObject, string> NonEqualValueObjects = new TheoryData<IValueObject, IValueObject, string>
        {
            {
                new ValueObjectA(A: 1, B: "2", C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(2, "3")),
                new ValueObjectA(A: 2, B: "2", C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(2, "3")),
                "they should not be equal because the 'A' member on ValueObjectA is different among them"
            },
            {
                new ValueObjectA(A: 1, B: "2", C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(2, "3")),
                new ValueObjectA(A: 1, B: null, C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(2, "3")),
                "they should not be equal because the 'B' member on ValueObjectA is different among them"
            },
            {
                new ValueObjectA(A: 1, B: "2", C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(A: 2, B: "3")),
                new ValueObjectA(A: 1, B: "2", C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(A: 3, B: "3")),
                "they should not be equal because the 'A' member on ValueObjectA's 'D' member is different among them"
            },
            {
                new ValueObjectA(A: 1, B: "2", C: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), D: new ComplexObject(A: 2, B: "3")),
                new ValueObjectB(a: 1, b: "2"),
                "they should not be equal because they are not of the same type"
            },
            {
                new ValueObjectB(1, "2",  1, 2, 3 ),
                new ValueObjectB(1, "2",  1, 2, 3, 4 ),
                "they should be not be equal because the 'C' list contains one additional value"
            },
            {
                new ValueObjectB(1, "2",  1, 2, 3, 5 ),
                new ValueObjectB(1, "2",  1, 2, 3 ),
                "they should be not be equal because the 'C' list contains one additional value"
            },
            {
                new ValueObjectB(1, "2",  1, 2, 3, 5 ),
                new ValueObjectB(1, "2",  1, 2, 3, 4 ),
                "they should be not be equal because the 'C' lists are not equal"
            }

        };

        private record ValueObjectA(
            int A, 
            string B, 
            Guid C, 
            ComplexObject D) 
            : IValueObject;

        private record ValueObjectB : IValueObject
        {
            public ValueObjectB(int a, string b, params int[] c)
            {
                A = a;
                B = b;
                C = c.ToList();
            }

            public int A { get; init; }
            public string B { get; init; }

            public List<int> C { get; init; }
        }

        private record ComplexObject(
            int A, 
            string B) 
            : IValueObject;
    }
}
