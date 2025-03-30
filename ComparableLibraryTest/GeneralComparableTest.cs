using Xunit.Sdk;

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
    }
}
