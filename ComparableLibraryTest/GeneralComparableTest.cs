using System.Globalization;
using ComparableLibrary.Utils;

namespace ComparableLibraryTest
{
    public class GeneralComparableTest
    {
        [Fact]
        public void HashSum_Should_Be_Different_For_Different_Values()
        {
            var obj1 = new SampleComparable
            {
                TextProperty = "Hello",
                NumberProperty = 5,
                Price = 199.99m,
                IsEnabled = true,
                Timestamp = new DateTime(2022, 12, 25),
                Duration = TimeSpan.FromMinutes(90),
                OptionalNumber = 8,
                OrderedNames = new List<string> { "Alice", "Bob", "Charlie" },
                UnorderedNames = new List<string> { "Charlie", "Alice", "Bob" },
                NestedObject = new NestedComparable { NestedText = "Nested", NestedNumber = 42 },
                AlternativeOrderList = new List<AlternativeComparable>()
                {
                    new AlternativeComparable { CustomText = "Test", CustomNumber = 100 },
                    new AlternativeComparable { CustomText = "Test2", CustomNumber = 101 }
                },
            };

            var obj2 = new SampleComparable
            {
                TextProperty = "World",
                NumberProperty = 10,
                Price = 299.99m,
                IsEnabled = false,
                Timestamp = new DateTime(2023, 1, 1),
                Duration = TimeSpan.FromMinutes(120),
                OptionalNumber = null,
                OrderedNames = new List<string> { "Eve", "Dan", "Frank" },
                UnorderedNames = new List<string> { "Frank", "Eve", "Dan" },
                NestedObject = new NestedComparable { NestedText = "NestedDiff", NestedNumber = 99 },
                AlternativeOrderList = new List<AlternativeComparable>()
                {
                    new AlternativeComparable { CustomText = "Test3", CustomNumber = 102 },
                    new AlternativeComparable { CustomText = "Test4", CustomNumber = 103 }
                }
            };

            Assert.NotEqual(obj1.HashSum, obj2.HashSum);
        }


        [Fact]
        public void HashSum_Should_Be_Same_For_Identical_Objects()
        {
            var obj1 = new SampleComparable
            {
                TextProperty = "Test",
                NumberProperty = 15,
                OrderedNames = new List<string> { "A", "B", "C" },
                UnorderedNames = new List<string> { "X", "Y", "Z" },
                NestedObject = new NestedComparable { NestedText = "Inside", NestedNumber = 123 },
                AlternativeOrderList = new List<AlternativeComparable>()
                {
                    new AlternativeComparable { CustomText = "Test", CustomNumber = 100 },
                    new AlternativeComparable { CustomText = "Test2", CustomNumber = 101 }
                },
                AlternativeUnorderList = new List<AlternativeComparable>()
                {
                    new AlternativeComparable { CustomText = "Test3", CustomNumber = 102 },
                    new AlternativeComparable { CustomText = "Test4", CustomNumber = 103 }
                }
            };

            var obj2 = new SampleComparable
            {
                TextProperty = "Test",
                NumberProperty = 15,
                OrderedNames = new List<string> { "A", "B", "C" },
                UnorderedNames = new List<string> { "Z", "X", "Y" },
                NestedObject = new NestedComparable { NestedText = "Inside", NestedNumber = 123 },
                AlternativeOrderList = new List<AlternativeComparable>()
                {
                    new AlternativeComparable { CustomText = "Test", CustomNumber = 100 },
                    new AlternativeComparable { CustomText = "Test2", CustomNumber = 101 }
                },
                AlternativeUnorderList = new List<AlternativeComparable>()
                {
                    new AlternativeComparable { CustomText = "Test4", CustomNumber = 103 },
                    new AlternativeComparable { CustomText = "Test3", CustomNumber = 102 }
                }
            };

            Assert.Equal(obj1.HashSum, obj2.HashSum);
        }

        [Fact]
        public void HashSum_Should_Ignore_Order_For_Unordered_Collections()
        {
            var obj1 = new SampleComparable { UnorderedNames = new List<string> { "A", "B", "C" } };
            var obj2 = new SampleComparable { UnorderedNames = new List<string> { "C", "B", "A" } };

            Assert.Equal(obj1.HashSum, obj2.HashSum);
        }

        [Fact]
        public void HashSum_Should_Compare_For_Similar_But_Different_Classes()
        {
            var obj1 = new NestedComparable { NestedText = "Test", NestedNumber = 100 };
            var obj2 = new AlternativeComparable { CustomText = "Test", CustomNumber = 100 };

            Assert.Equal(obj1.HashSum, obj2.HashSum);
        }

        public void HashSum_Should_Compare_For_Similar_But_Different_Classes_Through_Interfaces()
        {
            var obj1 = new NestedComparable { NestedText = "Test", NestedNumber = 100 };
            var obj2 = new AlternativeComparable { CustomText = "TestDiff", CustomNumber = 100 };

            var hash1 = obj1.GetHashSum();
            var hash2 = obj2.GetHashSum();


            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void HashSum_ThroughExtension_SameValues_Equals()
        {
            var a = new Person { Id = 1, Name = "Vlad" };
            var b = new Person { Id = 1, Name = "Vlad" };

            string ha = a.GetHashSum();
            string hb = b.GetHashSum();

            Assert.Equal(ha, hb);
        }

        [Fact]
        public void HashSum_UnorderedCollection_IgnoresOrder()
        {
            var a = new TagSet { Tags = new() { "a", "b", "c" } };
            var b = new TagSet { Tags = new() { "c", "b", "a" } };

            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void HashSum_OrderedCollection_RespectsOrder()
        {
            var a = new Sequence { Items = new() { "a", "b", "c" } };
            var b = new Sequence { Items = new() { "c", "b", "a" } };

            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void HashSum_NameMapping_MatchesAcrossTypes()
        {
            var a = new NameMappedA { KeyAA = "X" };
            var b = new NameMappedB { KeyBB = "X" };

            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }

        [Fact(Skip = "Enable after ensuring InvariantCulture formatting in implementation")]
        public void HashSum_IsCultureInvariant()
        {
            var obj = new Numbers { I = -123, D = 12345.6789 };

            var original = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("en-US");
                var h1 = obj.GetHashSum();

                CultureInfo.CurrentCulture = new CultureInfo("fr-FR");
                var h2 = obj.GetHashSum();

                Assert.Equal(h1, h2);
            }
            finally
            {
                CultureInfo.CurrentCulture = original;
            }
        }

        [Fact]
        public void HashSum_ListOfComplex_Ordered_RespectsOrder()
        {
            var a = new OrderedComplexList
            {
                Items = new()
                {
                    new OrderableItem { Id = 1, Name = "A" },
                    new OrderableItem { Id = 2, Name = "B" },
                    new OrderableItem { Id = 3, Name = "C" },
                }
            };

            var b = new OrderedComplexList
            {
                Items = new()
                {
                    new OrderableItem { Id = 3, Name = "C" },
                    new OrderableItem { Id = 2, Name = "B" },
                    new OrderableItem { Id = 1, Name = "A" },
                }
            };

            // Same elements, different order -> hashes must differ for Ordered collections
            Assert.NotEqual(a.GetHashSum(), b.GetHashSum());
        }

        [Fact]
        public void HashSum_ListOfComplex_Ordered_SameOrder_Equals()
        {
            var a = new OrderedComplexList
            {
                Items = new()
                {
                    new OrderableItem { Id = 1, Name = "A" },
                    new OrderableItem { Id = 2, Name = "B" },
                    new OrderableItem { Id = 3, Name = "C" },
                }
            };

            var b = new OrderedComplexList
            {
                Items = new()
                {
                    new OrderableItem { Id = 1, Name = "A" },
                    new OrderableItem { Id = 2, Name = "B" },
                    new OrderableItem { Id = 3, Name = "C" },
                }
            };

            // Identical sequence -> hashes must match
            Assert.Equal(a.GetHashSum(), b.GetHashSum());
        }
    }
}
