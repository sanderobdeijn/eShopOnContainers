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
                new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                new ValueObjectA(a: 2, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                "they should not be equal because the 'A' member on ValueObjectA is different among them"
            },
            {
                new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                new ValueObjectA(a: 1, b: null, c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3")),
                "they should not be equal because the 'B' member on ValueObjectA is different among them"
            },
            {
                new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3")),
                new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 3, b: "3")),
                "they should not be equal because the 'A' member on ValueObjectA's 'D' member is different among them"
            },
            {
                new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3")),
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

        private record ValueObjectA : IValueObject
        {
            public ValueObjectA(int a, string b, Guid c, ComplexObject d)
            {
                A = a;
                B = b;
                C = c;
                D = d;
            }

            public int A { get; }
            public string B { get; }
            public Guid C { get; }
            public ComplexObject D { get; }
        }

        private record ValueObjectB : IValueObject
        {
            public ValueObjectB(int a, string b, params int[] c)
            {
                A = a;
                B = b;
                C = c.ToList();
            }

            public int A { get; }
            public string B { get; }

            public List<int> C { get; }
        }

        private class ComplexObject : IEquatable<ComplexObject>
        {
            public ComplexObject(int a, string b)
            {
                A = a;
                B = b;
            }

            public int A { get; set; }

            public string B { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as ComplexObject);
            }

            public bool Equals(ComplexObject other)
            {
                return other != null &&
                       A == other.A &&
                       B == other.B;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(A, B);
            }
        }
    }
}
